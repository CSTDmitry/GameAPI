namespace API.Interface
{
  public interface IPlayer
  {
    public string PlayerName { get; set; }
    public string PlayerEmail { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
  }
}
