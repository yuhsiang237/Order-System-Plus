using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.Models.BusinessActor.Commands;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManageController : ControllerBase
    {
        private readonly UserManageCommandHandler _handler;

        public UserManageController(ILogger<UserManageController> logger,
            UserManageCommandHandler handler)
        {
            _handler = handler;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] ReqUserManageCreate req)
        {
            await _handler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("SignIn")]
        public async Task<RspSignInUser> SignInUser([FromBody] ReqSignInUser req)
            => await _handler.HandleAsync(req);

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] ReqUserUpdate req)
        {
            await _handler.HandleAsync(req);
            return StatusCode(200);
        }
    }
}