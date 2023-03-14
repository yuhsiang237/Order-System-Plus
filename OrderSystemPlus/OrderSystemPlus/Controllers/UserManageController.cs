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

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] ReqCreateUser req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("SignInUser")]
        public async Task<RspSignInUser> SignInUser([FromBody] ReqSignInUser req)
            => await _commandHandler.HandleAsync(req);

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] ReqUpdateUser req)
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