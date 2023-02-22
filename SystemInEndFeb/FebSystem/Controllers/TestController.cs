using EntityClass;
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



        [HttpGet]
        [Route("api/test2")]
        public IActionResult Test2([FromServices] IOptions<JwtOptions> jwt)
        {
             
            return Ok(jwt.Value.SigningKey);
        }



    }
}
