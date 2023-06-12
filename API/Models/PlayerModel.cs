using API.Interface;

namespace API.Models
{
  public class PlayerModel : IPlayer
  {
    public string PlayerName { get; set; } = string.Empty;
    public string PlayerEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
  }
}
