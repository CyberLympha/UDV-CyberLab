using MongoDB.Driver;
using WebApi.Models.LabWorks;

namespace WebApi.Services;

/// <summary>
/// Service for managing instruction steps.
/// </summary>
public class InstructionStepsService
{
    private readonly IMongoCollection<InstructionStep> instructionsStepsCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionStepsService"/> class.
    /// </summary>
    /// <param name="instructionsStepsCollection">The MongoDB collection of instruction steps.</param>
    public InstructionStepsService(IMongoCollection<InstructionStep> instructionsStepsCollection)
    {
        this.instructionsStepsCollection = instructionsStepsCollection;
    }
    
    /// <summary>
    /// Retrieves an instruction step by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the instruction step.</param>
    /// <returns>The instruction step with the specified identifier.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
    public async Task<InstructionStep> GetByIdAsync(string id)
    {
        try
        {
            return (await instructionsStepsCollection.FindAsync(bson => bson.Id == id)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}