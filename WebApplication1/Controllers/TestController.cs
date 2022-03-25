using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var connectionString =
            "Server=127.0.0.1;Database=Bootcamp;User Id=sa;Password=yourStrong(!)Password;";
                var connection = new SqlConnection(connectionString);
                connection.Open();
                var cmdText = "SELECT * FROM usuario";
                var sqlCommand = new SqlCommand(cmdText, connection);
                var usuarios = new List<Usuario>();
                
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Idade = Convert.ToInt32(reader["Idade"]),
                            Nome = Convert.ToString(reader["Nome"]),
                        };

                        usuarios.Add(usuario);
                    }
                }

                return Ok(usuarios);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
