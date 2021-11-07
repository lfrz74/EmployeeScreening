using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeScreening.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<ActionResult<string>> ConsultWeatherForecast(string guid);
    }
}
