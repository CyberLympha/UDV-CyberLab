using Microsoft.CodeAnalysis.Elfie.Serialization;
using MongoDB.Driver;
using WebApi.Models.LabWorks;

namespace WebApi.Services;

/// <summary>
///     Service for managing instruction steps.
/// </summary>
public class InstructionStepsService
{
    private readonly IMongoCollection<InstructionStep> instructionsStepsCollection;

    /// <summary>
    ///     Initializes a new instance of the <see cref="InstructionStepsService" /> class.
    /// </summary>
    /// <param name="instructionsStepsCollection">The MongoDB collection of instruction steps.</param>
    public InstructionStepsService(IMongoCollection<InstructionStep> instructionsStepsCollection)
    {
        this.instructionsStepsCollection = instructionsStepsCollection;
    }

    /// <summary>
    ///     Retrieves an instruction step by its identifier asynchronously.
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

    /// <summary>
    ///     Creates a new instruction step.
    /// </summary>
    /// <param name="newStep">The instruction step to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="newStep" /> is null.</exception>
    public async Task CreateAsync(InstructionStep newStep)
    {
        if (newStep == null) throw new Exception();
        try
        {
            await instructionsStepsCollection.InsertOneAsync(newStep);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///     Updates an existing instruction step.
    /// </summary>
    /// <param name="newStep">The instruction step to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="newStep" /> is null.</exception>
    /// <exception cref="ColumnNotFoundException">Thrown when the instruction step to update is not found.</exception>
    public async Task UpdateAsync(InstructionStep newStep)
    {
        var stepToUpdate = await instructionsStepsCollection.FindAsync(bson => bson.Id == newStep.Id) ??
                           throw new ColumnNotFoundException();
        try
        {
            var filter = Builders<InstructionStep>.Filter.Eq("Id", newStep.Id);
            var update = Builders<InstructionStep>.Update
                .Set("Instruction", newStep.Instruction)
                .Set("Hint", newStep.Hint)
                .Set("Answers", newStep.Answers);
            await instructionsStepsCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    ///     Deletes an instruction step by its identifier.
    /// </summary>
    /// <param name="stepId">The identifier of the instruction step to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ColumnNotFoundException">Thrown when the instruction step to delete is not found.</exception>
    public async Task DeleteAsync(string stepId)
    {
        var step = await GetByIdAsync(stepId) ??
                   throw new ColumnNotFoundException();
        try
        {
            await instructionsStepsCollection.DeleteOneAsync(bson => bson.Id == stepId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    ///     Retrieves all instruction steps.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of all instruction steps.</returns>
    public async Task<List<InstructionStep>> GetAllAsync()
    {
        try
        {
            return await instructionsStepsCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}