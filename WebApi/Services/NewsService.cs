using Corsinvest.ProxmoxVE.Api;
using MongoDB.Driver;
using WebApi.Models;

namespace WebApi.Services;

public class NewsService
{
    private readonly IMongoCollection<News> _newsCollection;

    public NewsService(
        IMongoCollection<News> newsCollection)
    {
        _newsCollection = newsCollection;
    }

    public async Task<List<News>> GetNews(int offset)
    {
        return await _newsCollection.Find(_ => true).SortBy(bson => bson.CreatedAt)
            .ThenByDescending(bson => bson.CreatedAt).Skip(offset).Limit(10).ToListAsync();
    }

    public async Task CreateNews(News news)
    {
        try
        {
            await _newsCollection.InsertOneAsync(news);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateNewItem(News news)
    {
        try
        {
            var update = Builders<News>.Update.Set("title", news.Title).Set("text", news.Text);
            await _newsCollection.FindOneAndUpdateAsync(bson => bson.Id == news.Id, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<News> GetNewItem(string id)
    {
        try
        {
            return (await _newsCollection.FindAsync(bson => bson.Id == id)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}