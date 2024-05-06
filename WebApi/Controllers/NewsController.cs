using Microsoft.AspNetCore.Mvc;
using WebApi.Model.NewsModel;
using WebApi.Model.NewsModel.Requests;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsService newsService;

        public NewsController(NewsService newsService)
        {
            this.newsService = newsService;
        }

        [HttpGet(Name = "GetNews")]
        public async Task<ActionResult<List<News>>> GetNews(int offset)
        {
            return await newsService.GetNews(offset);
        }

        [HttpPost(Name = "CreateNews")]
        public async Task<ActionResult> CreateNews(NewsCreateRequest news)
        {
            try
            {
                await newsService.CreateNews(new News()
                    { Title = news.title, Text = news.text, CreatedAt = news.createdAt });
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("item")]
        public async Task<ActionResult<News>> GetNewItem(string id)
        {
            try
            {
        
                return await newsService.GetNewItem(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost("update")]
        public async Task<ActionResult<News>> UpdateNewItem(News newItem)
        {
            try
            {
                await newsService.UpdateNewItem(newItem);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}