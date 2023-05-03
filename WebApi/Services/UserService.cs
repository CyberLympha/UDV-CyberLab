using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Models;

namespace WebApi.Services;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(
        IMongoCollection<User> usersCollection)
    {
        _usersCollection = usersCollection;
    }

    public async Task<List<User>> GetUsersAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string email) =>
        await _usersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

    public async Task<User> GetAsyncById(string id) =>
        await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser)
    {
        var candidate = await _usersCollection.CountDocumentsAsync(new BsonDocument("email", newUser.Email));

        if (candidate == 0)
        {
            await _usersCollection.InsertOneAsync(newUser);
        }
        else
        {
            throw new Exception("User already exist");
        }
    }

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _usersCollection.DeleteOneAsync(x => x.Id == id);
}