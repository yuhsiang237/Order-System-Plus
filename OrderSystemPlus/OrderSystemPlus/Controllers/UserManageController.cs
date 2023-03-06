using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.BusinessActor.Queries;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManageController : ControllerBase
    {
        private readonly UserManageCommandHandler _commandHandler;
        private readonly IUserManageQueryHandler _queryHandler;

        public UserManageController(
            UserManageCommandHandler commandHandler,
            IUserManageQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        [HttpPost("UserCreate")]
        public async Task<IActionResult> CreateUser([FromBody] ReqUserCreate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UserSignIn")]
        public async Task<RspUserSignIn> SignInUser([FromBody] ReqUserSignIn req)
            => await _commandHandler.HandleAsync(req);

        [HttpPost("UserUpdate")]
        public async Task<IActionResult> UpdateUser([FromBody] ReqUserUpdate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetUserList")]
        public async Task<List<RspGetUserList>> GetUserList([FromBody] ReqGetUserList req)
            => await _queryHandler.GetUserListAsync(req);

        [HttpPost("GetUserInfo")]
        public async Task<RspGetUserInfo> GetUserInfo([FromBody] ReqGetUserInfo req)
            => await _queryHandler.GetUserInfoAsync(req);

    }
}