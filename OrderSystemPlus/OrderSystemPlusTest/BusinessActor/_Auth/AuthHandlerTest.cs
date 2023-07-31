using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlusTest.BusinessActor
{
    public class AuthHandlerTest
    {
        private IAuthHandler _handler;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IJwtHelper> _jwtHelpMock;
        private readonly IConfiguration _configuration;
        public AuthHandlerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _jwtHelpMock = new Mock<IJwtHelper>();
            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:SecretKey", "testkey123testkey123"},
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _handler = new AuthHandler(
              _userRepository.Object,
              _jwtHelpMock.Object,
              _configuration);
        }

        [Fact]
        public async Task SignIn()
        {

            _userRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<UserDto> {
                new UserDto{
                    Account = "testAccount",
                    Password = "ZvczkadhFgUUPBY3r5N5f5RGFvCb4s3ptNzB3OoQWLpN5iBe5uPUrMvDsIR7hsUPSJktB2mvta+YzxICKYQO0Q==",
                    Salt = "*lZzd8@KB8*[/v]"
                }});

            _jwtHelpMock
                .Setup(x => x.GenerateAccessToken(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdHJpbmciLCJqdGkiOiJjM2FiYzgyYy03ZGU1LTQ5ODgtYjRmNy1kZmIxZDNlNGU0YjMiLCJuYmYiOjE2Nzc3MjU0MjgsImV4cCI6MTY3NzcyNzIyOCwiaWF0IjoxNjc3NzI1NDI4LCJpc3MiOiJKd3RBdXRoIn0.fWcFRo7G5q7Ro5imY-QOtdJvL1_8EcNOuFV_HA-QbAo");
            _jwtHelpMock
             .Setup(x => x.GenerateRefreshToken(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>()))
             .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdHJpbmciLCJqdGkiOiJjM2FiYzgyYy03ZGU1LTQ5ODgtYjRmNy1kZmIxZDNlNGU0YjMiLCJuYmYiOjE2Nzc3MjU0MjgsImV4cCI6MTY3NzcyNzIyOCwiaWF0IjoxNjc3NzI1NDI4LCJpc3MiOiJKd3RBdXRoIn0.fWcFRo7G5q7Ro5imY-QOtdJvL1_8EcNOuFV_HA-QbAo");

            var rsp = await _handler.HandleAsync(new ReqSignIn
            {
                Password = "testpwd605",
                Account = "testAccount",
            });

            rsp.AccessToken.Should().NotBeNullOrEmpty();

            _jwtHelpMock.Verify(x => x.GenerateAccessToken(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>()), Times.Once());
            _jwtHelpMock.Verify(x => x.GenerateRefreshToken(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>()), Times.Once());
            _userRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task RefreshAccessToken()
        {
            JwtHelper jwtHelper = new JwtHelper(_configuration);
            var handler = new AuthHandler(
             _userRepository.Object,
             jwtHelper,
             _configuration);
            var refreshToken = jwtHelper.GenerateRefreshToken("1", "accounttest", 14);
            var rsp = await handler.HandleAsync(new ReqRefreshAccessToken
            {
                RefreshToken = refreshToken,
            });
            rsp.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ValidateAccessToken()
        {
            JwtHelper jwtHelper = new JwtHelper(_configuration);
            var handler = new AuthHandler(
             _userRepository.Object,
             jwtHelper,
             _configuration);

            var accessToken = jwtHelper.GenerateAccessToken("1", "accounttest", 15);
            var rsp = await handler.HandleAsync(new ReqValidateAccessToken
            {
                AccessToken = accessToken,
            });
            rsp.Should().BeTrue();
        }
    }
}

