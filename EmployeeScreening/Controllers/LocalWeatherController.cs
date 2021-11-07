using EmployeeScreening.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EmployeeScreening.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalWeatherController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService;

        public LocalWeatherController(IWeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
        }

        /// <summary>
        /// Consult external weather forecast service
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public async Task<ActionResult> Get(string guid)
        {
            try
            {
                var res = await _weatherForecastService.ConsultWeatherForecast(guid);
                return Ok(res.Value);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
