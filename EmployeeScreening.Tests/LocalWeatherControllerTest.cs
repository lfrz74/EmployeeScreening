using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

using EmployeeScreening.Controllers;
using EmployeeScreening.Interfaces;
using EmployeeScreening.Models;
using EmployeeScreening.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EmployeeScreening.Tests
{
    [TestClass]
    public class LocalWeatherControllerTest
    {
        private static string GUID1 = "7044b64a-0f29-490c-b041-6b0c67ad6172";
        private static string GUID2 = "abcde1e5-ffed-4f10-905e-846b3e049932";

        private IWeatherForecastService _weatherForecastService;

        [TestMethod]
        public async Task Given_an_existing_GUID_return_a_valid_result()
        {
            using (var factory = new DbContextFactory())
            {
                await InitializeData(factory);
                // Get another context using the same connection
                using (var context = factory.CreateContext())
                {
                    _weatherForecastService = new WeatherForecastService(context);
                    //Controller
                    var controller = new LocalWeatherController(_weatherForecastService);

                    //test method
                    var response = await controller.Get(context.Users.FirstOrDefault(u => u.id.Equals(GUID1)).id);
            
                    //Results
                    Assert.AreEqual(200, ((ObjectResult)response).StatusCode);
                }
            }
        }

        [TestMethod]
        public async Task Given_an_existing_GUID_return_404_for_data_unavailable_for_requested_point()
        {
            using (var factory = new DbContextFactory())
            {
                await InitializeData(factory);
                // Get another context using the same connection
                using (var context = factory.CreateContext())
                {
                    _weatherForecastService = new WeatherForecastService(context);
                    //Controller
                    var controller = new LocalWeatherController(_weatherForecastService);

                    //test method
                    var response = await controller.Get(context.Users.FirstOrDefault(u => u.id.Equals(GUID2)).id);
                    JObject json = JObject.Parse(((ObjectResult)response).Value.ToString());
                    int res = int.Parse(json.GetValue("status").ToString());

                    //Results
                    Assert.AreEqual(404, res);
                }
            }
        }


        [TestMethod]
        public async Task Given_a_non_existent_GUID_return_not_found()
        {
            using (var factory = new DbContextFactory())
            {
                await InitializeData(factory);
                // Get another context using the same connection
                using (var context = factory.CreateContext())
                {
                    _weatherForecastService = new WeatherForecastService(context);
                    //Controller
                    var controller = new LocalWeatherController(_weatherForecastService);

                    //test method
                    var response = await controller.Get("a7982083-105f-46d6-a389-b8a168b90c43");

                   //Results
                    Assert.AreEqual("GUID not found!", ((ObjectResult)response).Value.ToString());
                }
            }
        }

        private async Task InitializeData(DbContextFactory dbContextFactory)
        {
            // Get a context
            using (var context = dbContextFactory.CreateContext())
            {
                var user1 = new User()
                {
                    id = GUID1,
                    name = "Bob Sage",
                    longitude = -80.191788m,
                    latitude = 25.761681m
                };
                var user2 = new User()
                {
                    id = GUID2,
                    name = "Luis Rivera",
                    longitude = -78.48293m,
                    latitude = -0.11220m
                };

                context.Users.Add(user1);
                context.Users.Add(user2);

                await context.SaveChangesAsync();
            }
        }
    }
}

