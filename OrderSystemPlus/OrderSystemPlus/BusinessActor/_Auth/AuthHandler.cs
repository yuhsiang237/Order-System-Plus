using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.HashSaltTool;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlus.BusinessActor
{
    public class AuthHandler : IAuthHandler
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;

        public AuthHandler(
            IUserRepository userRepository,
            IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<(string AccessToken, string RefreshToken)> HandleAsync(ReqSignIn req)
        {
            var user = (await _userRepository.FindByOptionsAsync(null, null, req.Account))
                .FirstOrDefault() ?? new UserDto();
            var isValid = HashSaltTool.Validate(req.Password,
                                                    user.Salt,
                                                    user.Password);
            if (isValid)
            {
                var refreshToken = _jwtHelper.GenerateRefreshToken(user.Id.ToString(), user.Account);
                var accessToken = _jwtHelper.GenerateAccessToken(user.Id.ToString(), user.Account, 1);

                return (accessToken, refreshToken);
            }
            else
            {
                throw new BusinessException("登入失敗，請確認帳號密碼是否正確。");
            }
        }

        public async Task HandleAsync(ReqRefreshAccessToken req)
        {

        }

        public async Task HandleAsync(ReqValidateAccessToken req)
        {
        }
    }
}
