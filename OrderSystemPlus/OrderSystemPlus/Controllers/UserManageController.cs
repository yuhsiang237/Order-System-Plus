using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManageController : ControllerBase
    {
        private readonly IUserManageHandler _userHandler;

        public UserManageController(
            IUserManageHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [HttpPost("CreateUser")]
        public async Task<int> CreateUser([FromBody] ReqCreateUser req)
        {
            return await _userHandler.HandleAsync(req);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] ReqUpdateUser req)
        {
            await _userHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] ReqDeleteUser req)
        {
            await _userHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetUserInfo")]
        public async Task<RspGetUserInfo> GetUserInfo([FromBody] ReqGetUserInfo req)
        {
            return await _userHandler.GetUserInfoAsync(req);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("GetUserList")]
        public async Task<List<RspGetUserList>> GetUserList([FromBody] ReqGetUserList req)
            => await _userHandler.GetUserListAsync(req);
    }
}