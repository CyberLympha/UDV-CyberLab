using Microsoft.CodeAnalysis.Elfie.Serialization;
using MongoDB.Driver;
using WebApi.Models;
using WebApi.Models.LabWorks;

namespace WebApi.Services;

/// <summary>
/// Service for managing user lab results.
/// </summary>
public class UserLabResultsService
{
    private readonly IMongoCollection<UserLabResult> userLabResults;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLabResultsService"/> class.
    /// </summary>
    /// <param name="userLabResults">The MongoDB collection of user lab results.</param>
    public UserLabResultsService(IMongoCollection<UserLabResult> userLabResults)
    {
        this.userLabResults = userLabResults;
    }

    /// <summary>
    /// Retrieves the user lab result asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="labId">The ID of the lab work.</param>
    /// <returns>The user lab result with the specified user and lab IDs.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
    public async Task<UserLabResult?> GetAsync(string userId, string labId)
    {
        try
        {
            return (await userLabResults.FindAsync(bson => bson.UserId == userId && bson.LabWorkId == labId)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Updates the current step number for a user's lab work asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="labWorkId">The ID of the lab work.</param>
    /// <param name="number">The step number to update.</param>
    /// <param name="stepsAmount">The amount of steps in the instruction</param>
    /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
    public async Task UpdateStepNumberAsync(string userId, string labWorkId, int number, int stepsAmount)
    {   
        await GetAsync(userId, labWorkId);
        try
        {
            var filterBuilder = Builders<UserLabResult>.Filter;
            var filter = filterBuilder.And(
                filterBuilder.Eq(result => result.UserId, userId),
                filterBuilder.Eq(result => result.LabWorkId, labWorkId)
            );
            var update = Builders<UserLabResult>.Update
                .Set("CurrentStep", number.ToString());
            if (stepsAmount == number)
                update.Set("IsFinished", true);
            
            await userLabResults.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Creates an initial user lab result record with the initial step and status.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="labWorkId">The identifier of the lab work.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the newly created <see cref="UserLabResult"/>.
    /// </returns>
    /// <exception cref="Exception">Thrown when there is an error inserting the new record into the collection.</exception>
    public async Task<UserLabResult> CreateInitialUserResultAsync(string userId, string labWorkId)
    {
        var newRecord = new UserLabResult()
        {
            CurrentStep = 0,
            IsFinished = false,
            UserId = userId,
            LabWorkId = labWorkId,
            Id = Guid.NewGuid().ToString()
        };
        try
        {
            await userLabResults.InsertOneAsync(newRecord);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return newRecord;
    }
    
    /// <summary>
    /// Retrieves a <see cref="UserLabResult"/> by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the user lab result.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <see cref="UserLabResult"/>.
    /// </returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the user lab result.</exception>
    public async Task<UserLabResult> GetByIdAsync(string id)
    {
        try
        {
            return (await userLabResults.FindAsync(bson => bson.Id == id)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Creates a new <see cref="UserLabResult"/>.
    /// </summary>
    /// <param name="newResult">The new user lab result to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the new result is null or an error occurs while inserting the new result.</exception>
    public async Task CreateAsync(UserLabResult newResult)
    {
        if (newResult == null) throw new Exception();
        try
        {
            await userLabResults.InsertOneAsync(newResult);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    /// <summary>
    /// Updates an existing <see cref="UserLabResult"/>.
    /// </summary>
    /// <param name="newResult">The updated user lab result.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the result to update is not found or an error occurs while updating the result.</exception>
    public async Task UpdateAsync(UserLabResult newResult)
    {
        var resultToUpdate = await userLabResults.FindAsync(bson => bson.Id == newResult.Id) ??
                              throw new ColumnNotFoundException();
        try
        {
            var filter = Builders<UserLabResult>.Filter.Eq("Id", newResult.Id);
            var update = Builders<UserLabResult>.Update
                .Set("UserId", newResult.UserId)
                .Set("LabWorkId", newResult.LabWorkId)
                .Set("IsFinished", newResult.IsFinished)
                .Set("CurrentStep", newResult.CurrentStep);
            await userLabResults.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Deletes a <see cref="UserLabResult"/> by its identifier.
    /// </summary>
    /// <param name="resultId">The identifier of the user lab result to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the result to delete is not found or an error occurs while deleting the result.</exception>
    public async Task DeleteAsync(string resultId)
    {
        var result = await GetByIdAsync(resultId) ??
                      throw new ColumnNotFoundException();
        try
        {
            await userLabResults.DeleteOneAsync(bson => bson.Id == resultId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves all <see cref="UserLabResult"/> records.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of all <see cref="UserLabResult"/> records.
    /// </returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the results.</exception>
    public async Task<List<UserLabResult>> GetAllAsync()
    {
        try
        {
            return await userLabResults.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves a list of <see cref="UserLabResult"/> records for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of <see cref="UserLabResult"/> records for the specified user.
    /// </returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the user results.</exception>
    public async Task<List<UserLabResult>> GetUserResultsAsync(string userId)
    {
        try
        {
            return await (await userLabResults.FindAsync(bson => bson.UserId == userId)).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}