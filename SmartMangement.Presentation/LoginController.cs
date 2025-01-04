using Microsoft.AspNetCore.Mvc;
using SmartMangement.Domain.Models;

namespace SmartMangement.Presentation
{
    [ApiVersion("1")]
    [Route("api/SmartManagement/v{version:apiVersion}/login")]
    public class LoginController: BaseController
    {
        public LoginController()
        {
            
        }
        [HttpGet]
        [Route("GetUserById")]
        public async Task<ActionResult<UserEnity>> GetUser(string username, string password)
        {
            UserEnity user = new UserEnity();
            return Ok(user);
        }
        [HttpPost]
        [Route("SaveUser")]
        public async Task<ActionResult<UserEnity>> SaveUser([FromBody] UserEnity user, CancellationToken token)
        {
            return Created("",user);
        }
    }
}
