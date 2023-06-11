using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interface;
using System.Data.SqlClient;
using Dapper;

namespace API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClientController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    public ClientController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IClient>> GetClient()
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      var client = await connection.QueryAsync<IClient>("select * from clients");
      return Ok(client);
    }
  }
}
