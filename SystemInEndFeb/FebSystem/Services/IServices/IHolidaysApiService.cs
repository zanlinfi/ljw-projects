using EntityClass;

namespace FebSystem.Services.IServices
{
    public interface IHolidaysApiService
    {
        Task<List<Holiday>> GetHolidays(string countryCode, int year);
    }

}
