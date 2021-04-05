using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly IDbConnection _dbConnection;

        public SampleController(
            ILogger<SampleController> logger,
            Func<string, IDbConnection> dbConnectionFactory)
        {
            _logger = logger;
            _dbConnection = dbConnectionFactory("Default");
        }

        [HttpGet("/things/{id}")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get([FromRoute] int id)
        {
            _dbConnection.Open();

            var results = await _dbConnection.QueryAsync<WeatherForecast>(
                "SELECT * FROM [Test] WHERE [Id] = @id",
                new {id});

            return Ok(results);
        }
    }
}
