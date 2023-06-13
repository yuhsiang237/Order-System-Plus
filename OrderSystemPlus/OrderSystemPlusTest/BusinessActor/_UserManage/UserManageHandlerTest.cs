using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;
using FluentAssertions;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlusTest.BusinessActor
{
    public class UserManageHandlerTest
    {
        private IUserManageHandler _handler;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IJwtHelper> _jwtHelp;

        public UserManageHandlerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _jwtHelp = new Mock<IJwtHelper>();

            _handler = new UserManageHandler(
              _userRepository.Object,
              _jwtHelp.Object);
        }

        [Fact]
        public async Task UserSignIn()
        {
            _userRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<UserDto> {
                new UserDto{
                    Account = "testAccount",
                    Password = "ZvczkadhFgUUPBY3r5N5f5RGFvCb4s3ptNzB3OoQWLpN5iBe5uPUrMvDsIR7hsUPSJktB2mvta+YzxICKYQO0Q==",
                    Salt = "*lZzd8@KB8*[/v]"
                }});

            _jwtHelp
                .Setup(x => x.GenerateToken(It.IsAny<string?>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdHJpbmciLCJqdGkiOiJjM2FiYzgyYy03ZGU1LTQ5ODgtYjRmNy1kZmIxZDNlNGU0YjMiLCJuYmYiOjE2Nzc3MjU0MjgsImV4cCI6MTY3NzcyNzIyOCwiaWF0IjoxNjc3NzI1NDI4LCJpc3MiOiJKd3RBdXRoIn0.fWcFRo7G5q7Ro5imY-QOtdJvL1_8EcNOuFV_HA-QbAo");

            var rsp = await _handler.HandleAsync(new ReqSignInUser
            {
                Password = "testpwd605",
                Account = "testAccount",
            });

            rsp.Token.Should().NotBeNullOrEmpty();

            _jwtHelp.Verify(x => x.GenerateToken(It.IsAny<string?>()), Times.Once());
            _userRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task UserCreate()
        {
            _userRepository.Setup(x => x.InsertAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(1);

            _userRepository
           .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
           .ReturnsAsync(new List<UserDto> {});

            await _handler.HandleAsync(new ReqCreateUser
            {
                Name = "userName",
                Email = "test@gmail.com",
                Account = "TEST",
                Password = "Test"
            });
            _userRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
            _userRepository.Verify(x => x.InsertAsync(It.IsAny<UserDto>()), Times.Once());
        }

        [Fact]
        public async Task GetUserListAsync()
        {
            _userRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<UserDto> {
                new UserDto{
                    Name = "Test",
                }});

            var rsp = await _handler.GetUserListAsync(new ReqGetUserList { });
            _userRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task GetUserInfoAsync()
        {
            _userRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<UserDto> {
                new UserDto{
                    Name = "Test",
                }});

            var rsp = await _handler.GetUserInfoAsync(new ReqGetUserInfo { });
            _userRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task UserUpdate()
        {
            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<UserDto>>()));

            await _handler.HandleAsync(   new ReqUpdateUser
                {
                    Id = 1,
                    Name = "userName",
                });
            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<UserDto>>()), Times.Once());
        }

        [Fact]
        public async Task UserDelete()
        {
            _userRepository.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<UserDto>>()));
            await _handler.HandleAsync(new ReqDeleteUser
            {
                Id = 1,
            });
            _userRepository.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<UserDto>>()), Times.Once());
        }
    }
}
