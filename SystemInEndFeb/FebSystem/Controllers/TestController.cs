using EntityClass;
using FebSystem.Services.IServices;
using IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace FebSystem.Controllers
{
    [ApiController]
    //[Authorize]
    public class TestController : ControllerBase
    {
        private readonly IHolidaysApiService _holidaysApiService;

        public TestController(IHolidaysApiService holidaysApiService)
        {
            _holidaysApiService = holidaysApiService;
        }

        /// <summary>
        /// identity framework test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test1")]
        public IActionResult Test1()
        {
            try
            {
                string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                return Ok($"id is {id}");
            }
            catch (Exception ex)
            {

                Console.WriteLine("no user login"+ex);
                return BadRequest();
            }
        }


        /// <summary>
        /// jwt test
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test2")]
        public IActionResult Test2([FromServices] IOptions<JwtOptions> jwt)
        {
             
            return Ok(jwt.Value.SigningKey);
        }

        
        [HttpGet]
        [Route("api/test3")]
        public async Task<IActionResult> Test3(string countryCode, int year)
        {
            List<Holiday> holidays = new List<Holiday>();
            holidays = await _holidaysApiService.GetHolidays(countryCode, year);

            return Ok(holidays);
        }

    }
}
