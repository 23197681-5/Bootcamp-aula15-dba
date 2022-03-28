using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IConfiguration Configuration;
        private readonly IConfiguration _configuration;

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DevConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var cmdText = "SELECT * FROM users";
                    var sqlCommand = new SqlCommand(cmdText, connection);
                    var users = new List<Usuario>();

                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Usuario()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Idade = Convert.ToInt32(reader["Age"]),
                                Nome = Convert.ToString(reader["Name"]),
                            };

                            users.Add(user);
                        }
                    }

                    // connection.Close();
                    // connection.Dispose();
                    return Ok(users.OrderByDescending(x => x.Nome));
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
