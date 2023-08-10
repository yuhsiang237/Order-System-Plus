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

        public UserManageHandlerTest()
        {
            _userRepository = new Mock<IUserRepository>();

            _handler = new UserManageHandler(
              _userRepository.Object);
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
