using Dapper.Db;
using EFCore.Db;
using EntityClass;
using FebSystem.Helper.Attritubes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;

namespace FebSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksDapperController : ControllerBase
    {
        private readonly DapperProvider ctx;
        private readonly ILogger<BooksDapperController> logger;
        private readonly IMemoryCache memoryCache;

        public BooksDapperController(ILogger<BooksDapperController> logger, DapperProvider ctx, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.ctx = ctx;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [NotUseTransaction]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("start get books");
            // use memory cache
            var data = await memoryCache.GetOrCreateAsync("AllBooks", async (e) =>
            {
                e.SlidingExpiration = TimeSpan.FromSeconds(9);
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(27);
                logger.LogInformation("get data from repository");
                return await ctx.Books.GetAllAsync();
            });
            logger.LogInformation("return data");
            return Ok(data);
        }


        [HttpGet("{id}")]
        [NotUseTransaction]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await ctx.Books.GetByIdAsync(id);
            if (data == null)
            {
                logger.LogInformation($"not found book {id}");
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]Book book)
        {
            var data = await ctx.Books.UpdateAsync(id, book);
            if (data == 0)
            {
                logger.LogInformation($"update book {id} failure");
                return BadRequest("update erro");
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book book)
        {
            var data = await ctx.Books.AddAsync(book);
            if (data == 0)
            {
                logger.LogInformation($"add failure");
                return BadRequest("add erro");
            }
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await ctx.Books.DeleteAsync(id);
            if (data == 0)
            {
                logger.LogInformation($"no this data");
                return BadRequest("delete failure");
            }
            return Ok(data);
        }

    }
}
