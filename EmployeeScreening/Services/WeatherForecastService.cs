using EmployeeScreening.DBContext;
using EmployeeScreening.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeScreening.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static string EXTERNAL_URI = "https://api.weather.gov/points";
        private static string USER_AGENT = "User-Agent";
        private static string STACKOVERFLOW = "Stackoverflow/1.0";

        private readonly ApplicationDBContext _applicationDBContext;

        public WeatherForecastService(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<ActionResult<string>> ConsultWeatherForecast(string guid)
        {
            string responseBody = string.Empty;
            Decimal long_VALUE = 0m;
            Decimal lat_VALUE = 0m;
            
            // Searching the record with EF
            var us = await _applicationDBContext.Users.FirstOrDefaultAsync(u => u.id.Equals(guid));
            if (us != null)
            {
                lat_VALUE = us.latitude;
                long_VALUE = us.longitude;
            }
            else
                throw new Exception("GUID not found!");

            //Invoking external service
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(USER_AGENT, STACKOVERFLOW);
            string uri = $"{EXTERNAL_URI}/{lat_VALUE},{long_VALUE}";
            var response = await client.GetAsync(uri);
            responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
