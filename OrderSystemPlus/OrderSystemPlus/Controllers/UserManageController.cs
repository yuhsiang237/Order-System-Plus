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

        [HttpPost("UserCreate")]
        public async Task<IActionResult> CreateUser([FromBody] ReqUserCreate req)
        {
            await _handler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UserSignIn")]
        public async Task<RspUserSignIn> SignInUser([FromBody] ReqUserSignIn req)
            => await _handler.HandleAsync(req);

        [HttpPost("UserUpdate")]
        public async Task<IActionResult> UpdateUser([FromBody] ReqUserUpdate req)
        {
            await _handler.HandleAsync(req);
            return StatusCode(200);
        }
    }
}