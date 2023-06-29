using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interface;
using API.Models;
using System.Data.SqlClient;
using Dapper;

public interface IPlayerInfo
{
  int Id { get; set; }
  string PlayerName { get; set; }
  string? PlayerToken { get; set; }
}

namespace API.Controllers
{
  public class PlayerInfo : IPlayerInfo
  {
    public int Id { get; set; }
    public string PlayerName { get; set; } = string.Empty;

    public string? PlayerToken { get; set; } = string.Empty;
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
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));

      var players = await connection.QueryAsync<PlayerInfo>("SELECT PlayerName FROM Players");
      return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IPlayerInfo>> GetClientById(int id)
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      var player = await connection.QueryFirstAsync<PlayerInfo>("SELECT Id, PlayerName FROM Players WHERE Id = @id",
        new { id });
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

    [HttpPost("{email}/{password}")]
    public async Task<ActionResult> PlayerExists(string email, string password)
    {
      using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));

      Console.WriteLine(email, password);

      var player = await connection.QueryFirstOrDefaultAsync<PlayerInfo>(
        "SELECT Id, PlayerName FROM Players WHERE PlayerEmail = @email AND PasswordHash = @password",
        new { email, password });

      if (player != null)
      {
        player.PlayerToken = "qwertjjks123";

        return Ok(player);
      }
      else
      {
        return Ok(false);
      }
    }
  }
}
