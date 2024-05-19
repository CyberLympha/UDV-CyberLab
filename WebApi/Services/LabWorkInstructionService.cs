using MongoDB.Driver;
using WebApi.Models.LabWorks;
using WebApi.Models.Logs;
using WebApi.Services.Logs;

namespace WebApi.Services;

/// <summary>
/// Service for managing lab work instructions.
/// </summary>
public class LabWorkInstructionService
{
    private readonly IMongoCollection<LabWorkInstruction> labWorkInstructionsCollection;
    private readonly InstructionStepsService instructionStepsService;
    private readonly UserLabResultsService userLabResultsService;
    private readonly LabWorkService labWorkService;
    private readonly LogsReader logsReader;
    private readonly UserService userService;
    private readonly ProxmoxService proxmoxService;
    private readonly VmService vmService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LabWorkInstructionService"/> class.
    /// </summary>
    /// <param name="labWorkInstructionsCollection">The MongoDB collection of lab work instructions.</param>
    /// <param name="instructionStepsService">The service for managing instruction steps.</param>
    /// <param name="userLabResultsService">The service for managing user lab results.</param>
    /// <param name="labWorkService">The service for managing lab works.</param>
    /// <param name="logsReader">The service for reading logs.</param>
    /// <param name="userService">The service for managing users.</param>
    /// <param name="proxmoxService">The service for interacting with Proxmox.</param>
    /// <param name="vmService">The service for managing virtual machines.</param>
    public LabWorkInstructionService(
        IMongoCollection<LabWorkInstruction> labWorkInstructionsCollection,
        InstructionStepsService instructionStepsService,
        UserLabResultsService userLabResultsService,
        LabWorkService labWorkService,
        LogsReader logsReader,
        UserService userService,
        ProxmoxService proxmoxService,
        VmService vmService)
    {
        this.labWorkInstructionsCollection = labWorkInstructionsCollection;
        this.instructionStepsService = instructionStepsService;
        this.userLabResultsService = userLabResultsService;
        this.labWorkService = labWorkService;
        this.logsReader = logsReader;
        this.userService = userService;
        this.proxmoxService = proxmoxService;
        this.vmService = vmService;
    }
    
    /// <summary>
    /// Retrieves a lab work instruction asynchronously.
    /// </summary>
    /// <param name="instructionId">The ID of the instruction to retrieve.</param>
    /// <returns>The lab work instruction with the specified ID.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
    public async Task<LabWorkInstruction> GetAsync(string instructionId)
    {
        try
        {
            return (await labWorkInstructionsCollection.FindAsync(bson => bson.Id == instructionId)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the instruction text for a specific step asynchronously.
    /// </summary>
    /// <param name="instructionId">The ID of the instruction.</param>
    /// <param name="stepNumber">The number of the step.</param>
    /// <returns>The instruction text for the specified step.</returns>
    public async Task<string> GetStepInstructionAsync(string instructionId, string stepNumber)
    {
        var instruction = await GetAsync(instructionId);
        var stepId = instruction.Steps[stepNumber];
        var step = await instructionStepsService.GetByIdAsync(stepId);
        
        return step.Instruction;
    }
    
    /// <summary>
    /// Checks if a step is the last step in an instruction asynchronously.
    /// </summary>
    /// <param name="instructionId">The ID of the instruction.</param>
    /// <param name="stepNumber">The number of the step to check.</param>
    /// <returns>True if the step is the last step; otherwise, false.</returns>
    public async Task<bool> IsStepLastAsync(string instructionId, string stepNumber)
    {
        var instruction = await GetAsync(instructionId);

        if (int.TryParse(stepNumber, out var stepIntNumber))
            return stepIntNumber == instruction.Steps.Count;
        
        return false;
    }

    /// <summary>
    /// Retrieves the hint for a specific step in an instruction asynchronously.
    /// </summary>
    /// <param name="instructionId">The ID of the instruction.</param>
    /// <param name="stepNumber">The number of the step.</param>
    /// <returns>The hint for the specified step.</returns>
    public async Task<string> GetStepHintAsync(string instructionId, string stepNumber)
    {
        var instruction = await GetAsync(instructionId);
        var stepId = instruction.Steps[stepNumber];
        var step = await instructionStepsService.GetByIdAsync(stepId);
        
        return step.Hint;
    }
    
    /// <summary>
    /// Retrieves the number of steps in a lab work instruction asynchronously.
    /// </summary>
    /// <param name="instructionId">The ID of the lab work instruction.</param>
    /// <returns>The number of steps in the instruction.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
    public async Task<int> GetStepsAmountAsync(string instructionId)
    {
        var instruction = await GetAsync(instructionId);
        
        return instruction.Steps.Count;
    }
    
    /// <summary>
    /// Checks the user's answer for a lab work task asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="labWorkId">The ID of the lab work.</param>
    /// <param name="stepNumber">The number of the step.</param>
    /// <returns>True if the answer is correct; otherwise, false.</returns>
    public async Task<bool> CheckAnswer(string userId, string labWorkId, string stepNumber)
    {
        if (!int.TryParse(stepNumber, out var stepIntNumber))
            return false;

        var labWork = await labWorkService.GetByIdAsync(labWorkId);
        var instruction = await GetAsync(labWork.InstructionId);
        var instructionStepId = instruction.Steps[stepNumber];
        var instructionStep = await instructionStepsService.GetByIdAsync(instructionStepId);
        var user = await userService.GetAsyncById(userId);
        var vmId = await vmService.GetVmId(user.VmId);
        var correctAnswers = instructionStep.Answers;
        var userLogs = await logsReader.ReadLogs(instruction.LogFilePaths, vmId);
        var userLabResult = await userLabResultsService.GetAsync(userId, labWorkId) 
                            ?? await userLabResultsService.CreateInitialUserResult(userId, labWorkId);
        
        if (userLabResult.IsFinished || userLabResult.CurrentStep >= stepIntNumber)
        {
            await proxmoxService.ClearFilesContent(vmId, instruction.LogFilePaths.Values.ToList());
            return true;
        }

        if (!CheckAnswer(correctAnswers, userLogs)) return false;
        
        await proxmoxService.ClearFilesContent(vmId, instruction.LogFilePaths.Values.ToList());
        await userLabResultsService.UpdateStepNumberAsync(userId, labWorkId, stepIntNumber, instruction.Steps.Count);
        
        return true;
    }
    
    private static bool CheckAnswer(List<string> correctAnswers, List<Log> userLogs)
    {
        return correctAnswers.Any(correctAnswer =>
        {
            var correctAnswerLines = correctAnswer.Split('\n');
            return correctAnswerLines.Length <= userLogs.Count &&
                   CheckEachAnswerLine(correctAnswerLines, userLogs);
        });
    }

    private static bool CheckEachAnswerLine(string[] correctAnswerLines, List<Log> userLogs)
    {
        var currentIndexInCorrectAnswer = 0;
        foreach (var log in userLogs)
        {
            var correctAnswerLine = correctAnswerLines[currentIndexInCorrectAnswer];
            var correctAnswerWords = correctAnswerLine.Split(' ');
            
            if (CompareAnswerToLog(correctAnswerWords, log.Arguments))
            {
                currentIndexInCorrectAnswer++;
                if (currentIndexInCorrectAnswer >= correctAnswerLines.Length)
                    return true;
            }
        }

        return false;
    }

    private static bool CompareAnswerToLog(string[] answerWords, List<string> logArguments)
    {
        return answerWords.Length <= logArguments.Count 
               && !answerWords.Where((t, i) => t != logArguments[i]).Any();
    }
}