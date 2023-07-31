using EntityClass;
using FebSystem.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FebSystem.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ThirdPartyAPIController : ControllerBase
    {
        private readonly IHolidaysApiService _holidaysApiService;

        public ThirdPartyAPIController(IHolidaysApiService holidaysApiService)
        {
            _holidaysApiService = holidaysApiService;
        }
        [HttpGet]
        [Route("thirdParty/{countryCode}/{year}")]
        public async Task<List<Holiday>> Holidays(string countryCode, int year)
        {
            List<Holiday> holidays;
            holidays = await _holidaysApiService.GetHolidays(countryCode, year);

            return holidays;
        }

    }
}
