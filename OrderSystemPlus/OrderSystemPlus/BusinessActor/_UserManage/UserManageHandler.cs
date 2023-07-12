using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.HashSaltTool;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlus.BusinessActor
{
    public class UserManageHandler : IUserManageHandler
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private static SemaphoreSlim _userCreateSemaphoreSlim;

        public UserManageHandler(
            IUserRepository userRepository,
            IJwtHelper jwtHelper)
        {
            _userCreateSemaphoreSlim = new SemaphoreSlim(1, 1);
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<List<RspGetUserList>> GetUserListAsync(ReqGetUserList req)
        {
            var data = await _userRepository.FindByOptionsAsync(null, null, null);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, RspGetUserList>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<UserDto>, List<RspGetUserList>>(data);
            return rsp.ToList();
        }

        public async Task<RspGetUserInfo> GetUserInfoAsync(ReqGetUserInfo req)
        {
            var data = (await _userRepository.FindByOptionsAsync(id: req.Id)).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, RspGetUserInfo>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<UserDto, RspGetUserInfo>(data);
            return rsp;
        }

        public async Task<int> HandleAsync(ReqCreateUser req)
        {
            await _userCreateSemaphoreSlim.WaitAsync();
            try
            {
                var result = default(int);
                var now = DateTime.Now;
                var hashSalt = HashSaltTool.Generate(req.Password);
                var isExist = (await _userRepository.FindByOptionsAsync(null, null, req.Account))
                            .ToList()
                            .Any();
                if (isExist)
                    throw new BussinessException("使用者已存在");

                result = await _userRepository.InsertAsync(
                        new UserDto
                        {
                            Name = req.Name,
                            Salt = hashSalt.Salt,
                            Email = req.Email,
                            Account = req.Account,
                            Password = hashSalt.Hash,
                            RoleId = (int)RoleType.Basic,
                            IsValid = true,
                            CreatedOn = now,
                            UpdatedOn = now,
                        });

                return await Task.FromResult(result);
            }
            finally
            {
                _userCreateSemaphoreSlim.Release();
            }
        }

        public async Task HandleAsync(ReqUpdateUser req)
        {
            var now = DateTime.Now;
            var dto = new UserDto
            {
                Id = req.Id,
                Name = req.Name,
                Email = req.Email,
                UpdatedOn = now,
            };
            await _userRepository.UpdateAsync(
                new List<UserDto> { dto });
        }

        public async Task HandleAsync(ReqDeleteUser req)
        {
            var now = DateTime.Now;
            var dto = new UserDto
            {
                Id = req.Id,
                UpdatedOn = now,
            };
            await _userRepository.DeleteAsync(
                new List<UserDto> { dto });
        }

        public async Task<RspSignInUser> HandleAsync(ReqSignInUser req)
        {
            var user = (await _userRepository.FindByOptionsAsync(null, null, req.Account))
                .FirstOrDefault();
            var isValid = HashSaltTool.Validate(req.Password,
                                                    user.Salt,
                                                    user.Password);
            if (isValid)
            {
                return new RspSignInUser
                {
                    Token = _jwtHelper.GenerateToken(user.Account),
                };
            }
            else
            {
                return new RspSignInUser
                {
                    Token = String.Empty,
                };
            }
        }
    }
}
