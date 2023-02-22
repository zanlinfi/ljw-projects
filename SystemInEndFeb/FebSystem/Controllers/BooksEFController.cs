using Microsoft.AspNetCore.Mvc;
using EFCore.Db;
using EntityClass;
using Microsoft.AspNetCore.Authorization;
using FebSystem.Helper.Attritubes;
using Microsoft.Extensions.Caching.Memory;

namespace FebSystem.Controllers
{
    [ApiController]
    [Authorize]
    public class BooksEFController : ControllerBase
    {
        private readonly EFProvider ctx;
        private readonly ILogger<BooksEFController> logger;
        private readonly IMemoryCache memoryCache;
        

        public BooksEFController(ILogger<BooksEFController> logger, EFProvider ctx, IMemoryCache memoryCache)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        // query
        // GET: api/<BooksEFController>
        [HttpGet]
        [Route("api/books")]
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

        // GET api/<BookAdoController>/5
        [HttpGet()]
        [Route("api/books/{id}")]
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

        // POST api/<BookAdoController>
        [HttpPost]
        [Route("api/books")]
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

        // PUT api/<BookAdoController>/5
        [HttpPut()]
        [Route("api/books/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            var data = await ctx.Books.UpdateAsync(id, book);
            if (data == 0)
            {
                logger.LogInformation($"update book {id} failure");
                return BadRequest("update erro");
            }
            return Ok(data);
        }

        // DELETE api/<BookAdoController>/5
        [HttpDelete()]
        [Route("api/books/{id}")]
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
