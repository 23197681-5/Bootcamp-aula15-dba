using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
/*
 * httpPut => replace resource
 * httpPatch => partial changes
 */
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DevConnection");

        }
        private IConfiguration Configuration;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        [HttpPut]
        public IActionResult Put(int id, Usuario user)
        {   
            try
            {
                if(string.IsNullOrEmpty(user.Name) || user.Age < 1)
                    throw new ArgumentException();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var cmdText = "UPDATE users " +
                                  "SET" +
                                  "name='@name, " +
                                  "age=@age" +
                                  "WHERE Id = @Id" +
                                  "')";
                    var sqlCommand = new SqlCommand(cmdText, connection);
                    sqlCommand.Parameters.Add("@name", SqlDbType.Text, 100);
                    sqlCommand.Parameters["@name"].Value = user.Name;
                    sqlCommand.Parameters.Add("@age", SqlDbType.Text, 100);
                    sqlCommand.Parameters["@age"].Value = user.Age;
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Text, 100);
                    sqlCommand.Parameters["@age"].Value = user.Id;

                    var result = sqlCommand.ExecuteNonQuery();//insert update delete
                    return Ok(result);
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult Post(Usuario user)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var cmdText = "INSERT INTO users VALUES('@name, @age') SELECT SCOPE IDENTITY";
                    var sqlCommand = new SqlCommand(cmdText, connection);
                    sqlCommand.Parameters.Add("@name", SqlDbType.Text, 100);
                    sqlCommand.Parameters["@name"].Value = user.Name;
                    sqlCommand.Parameters.Add("@age", SqlDbType.Text, 100);
                    sqlCommand.Parameters["@age"].Value = user.Age;

                    var result = sqlCommand.ExecuteScalar();//insert update delete
                    return Ok(result);
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
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
                                Age = Convert.ToInt32(reader["Age"]),
                                Name = Convert.ToString(reader["Name"]),
                            };

                            users.Add(user);
                        }
                    }

                    // connection.Close();
                    // connection.Dispose();
                    return Ok(users.OrderByDescending(x => x.Name));
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
