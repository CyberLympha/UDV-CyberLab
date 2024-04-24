using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Models;

namespace WebApi.Services;

/// <summary>
/// Service for CRUD operations on laboratory work data stored in MongoDB.
/// </summary>
public class LabWorkService
{
    private readonly IMongoCollection<LabWork> labWorkCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="LabWorkService"/> class.
    /// </summary>
    /// <param name="labWorkCollection">The MongoDB collection used for storing laboratory work data.</param>
    public LabWorkService(IMongoCollection<LabWork> labWorkCollection)
    {
        this.labWorkCollection = labWorkCollection;
    }
    
    /// <summary>
    /// Creates a new laboratory work record.
    /// </summary>
    /// <param name="newLabWork">The laboratory work record to create.</param>
    public async Task CreateAsync(LabWork newLabWork)
    {
        if (newLabWork == null) throw new Exception();
        try
        {
            await labWorkCollection.InsertOneAsync(newLabWork);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a laboratory work record by its ID.
    /// </summary>
    /// <param name="id">The ID of the laboratory work to retrieve.</param>
    /// <returns>The laboratory work record.</returns>
    public async Task<LabWork> GetByIdAsync(string id)
    {
        try
        {
            return (await labWorkCollection.FindAsync(bson => bson.Id == id)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing laboratory work record.
    /// </summary>
    /// <param name="labWork">The updated laboratory work record.</param>
    public async Task UpdateAsync(LabWork labWork)
    {
        var labWorkToUpdate = await labWorkCollection.FindAsync(bson => bson.Id == labWork.Id) ??
            throw new Exception("LabWork not found");
        try
        {
            var filter = Builders<LabWork>.Filter.Eq("Id", labWork.Id);
            var update = Builders<LabWork>.Update
                .Set("Title", labWork.Title)
                .Set("ShortDescription", labWork.ShortDescription)
                .Set("Description", labWork.Description)
                .Set("VmId", labWork.VmId);
            await labWorkCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Deletes a laboratory work record by its ID.
    /// </summary>
    /// <param name="labWorkId">The ID of the laboratory work to delete.</param>
    public async Task DeleteAsync(string labWorkId)
    {
        var labWork = await GetByIdAsync(labWorkId) ??
                throw new Exception("Reservation not found");
        try
        {
            await labWorkCollection.DeleteOneAsync(bson => bson.Id == labWorkId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all laboratory work records.
    /// </summary>
    /// <returns>A list of all laboratory work records.</returns>
    public async Task<List<LabWork>> GetAllAsync()
    {
        try
        {
            return await labWorkCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}