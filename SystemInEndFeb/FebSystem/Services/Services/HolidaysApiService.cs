using EntityClass;
using FebSystem.Services.IServices;
using System.Text.Json;

namespace FebSystem.Services.Services
{
    public class HolidaysApiService : IHolidaysApiService
    {
        private readonly HttpClient client;

        public HolidaysApiService(IHttpClientFactory clientFactory)
        {
            client = clientFactory.CreateClient("PublicHolidaysApi");
        }

        public async Task<List<Holiday>> GetHolidays(string countryCode, int year)
        {
            var url = string.Format("/api/v2/PublicHolidays/{0}/{1}", year, countryCode);
            var result = new List<Holiday>();
            var response = await client.GetAsync(url);// call api
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<Holiday>>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return result;
        }
    }


}
