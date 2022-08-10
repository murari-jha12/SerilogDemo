using Microsoft.AspNetCore.Mvc;
using SerilogDemo.Model;

namespace SerilogDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoggerDemoController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<LoggerDemoController> _logger;

        public LoggerDemoController(ILogger<LoggerDemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Info Logging...");
            _logger.LogError("Error Logging...");
            _logger.LogWarning("Warning Logging..");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "GetStudents")]
        public ActionResult<StudentInfo> GetStudents()
        {
            var studentInfo = new StudentInfo
            {
                Id = 101,
                Name = "Aman",
                Age = 15,
                MobileNo = "99998881",
                Address = "Delhi"
            };
            //for object logging we must use @
            _logger.LogInformation("Getting request {@student}", studentInfo);

            // for single property logging 
            _logger.LogInformation("Student's age {studentAge}", studentInfo.Age);
            _logger.LogInformation("Student's Address {address}", studentInfo.Address);

            return studentInfo;
        }

        [HttpGet(Name = "GetErrorLogged")]
        public ActionResult<StudentInfo> GetErrorLogged()
        {
            var studentInfo = new StudentInfo();
            try
            {
                studentInfo = null;
                //for object logging we must use @
                _logger.LogInformation("Getting request {@student}", studentInfo);

                // for single property logging 
                _logger.LogInformation("Student's age {studentAge}", studentInfo.Age);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting student Info {@exception}", ex);
            }
            return studentInfo;
        }
    }
}