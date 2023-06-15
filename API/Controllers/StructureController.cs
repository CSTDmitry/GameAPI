using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class PlayerAccess
  {
    public string PlayerName { get; set; } = string.Empty;
    public string PlayerKey { get; set; } = string.Empty;
  }
}

namespace API.Controllers
{
  [Route("api/structures/[controller]")]
  [ApiController]
  public class StructureController : ControllerBase
  {
    [HttpGet("{name}, {key}")]
    public async Task<ActionResult> GetAll(string name, string key)
    {
      return Ok($"Test structures: {name} : {key}");
    }
  }
}
