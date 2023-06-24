using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interface;
using API.Models;
using System.Data.SqlClient;
using Dapper;

public interface IPlayerInfo
{
  string PlayerName { get; set; }
}

namespace API.Controllers
{
  public class PlayerInfo : IPlayerInfo
  {
    public string PlayerName { get; set; } = string.Empty;
  }
}

  namespace API.Controllers
{
  [Route("api/player")]
  [ApiController]
  public class PlayerController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    public PlayerController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<IPlayerInfo>>> GetPlayers()
    {
      Console.WriteLine("Request");

      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      var players = await connection.QueryAsync<PlayerInfo>("SELECT PlayerName FROM Players");
      return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IPlayerInfo>> GetClientById(int id)
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      var player = await connection.QueryFirstAsync<PlayerInfo>("SELECT PlayerName FROM Players WHERE Id = @id",
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
