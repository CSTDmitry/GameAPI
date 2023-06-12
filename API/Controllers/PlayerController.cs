using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interface;
using API.Models;
using System.Data.SqlClient;
using Dapper;

namespace API.Controllers
{
  [Route("api/player/[controller]")]
  [ApiController]
  public class PlayerController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    public PlayerController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IPlayer>> GetClient()
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      var client = await connection.QueryAsync<IPlayer>("select * from Players");
      return Ok(client);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<IPlayer>> GetClientById(int id)
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      var player = await connection.QueryFirstAsync<IPlayer>("SELECT PlayerName FROM Players WHERE Id = @id",
        new { Id = id });
      return Ok(player);
    }

    [HttpPost]
    public async Task<ActionResult> Create(PlayerModel player)
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      await connection.ExecuteAsync(
          "INSERT INTO Players (PlayerName, PlayerEmail, PasswordHash, PasswordSalt)" +
          "VALUES (@PlayerName, @PlayerEmail, @PasswordHash, @PasswordSalt)",
          new { player.PlayerName, player.PlayerEmail, PasswordHash = "123", PasswordSalt = "432" }
        );

      return Ok();
    }
  }
}
