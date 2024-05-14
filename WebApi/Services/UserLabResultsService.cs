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
    public async Task<UserLabResult> GetAsync(string userId, string labId)
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
    /// <exception cref="Exception">Thrown when an error occurs during the operation.</exception>
    public async Task UpdateStepNumberAsync(string userId, string labWorkId, string number)
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
                .Set("CurrentStep", number);
            
            await userLabResults.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}