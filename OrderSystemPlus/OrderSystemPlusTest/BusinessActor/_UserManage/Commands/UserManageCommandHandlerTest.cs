using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using FluentAssertions;
using Moq;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Queries;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlusTest.DataAccessor
{
    public class UserManageCommandHandlerTest
    {
        private UserManageCommandHandler _handler;
        private readonly Mock<IUserQuery> _query;
        private readonly Mock<IJwtHelper> _jwtHelp;
        private readonly Mock<IInsertCommand<IEnumerable<UserCommandModel>>> _insertMock;
        private readonly Mock<IUpdateCommand<IEnumerable<UserCommandModel>>> _updateMock;

        public UserManageCommandHandlerTest()
        {
            _insertMock = new Mock<IInsertCommand<IEnumerable<UserCommandModel>>>();
            _updateMock = new Mock<IUpdateCommand<IEnumerable<UserCommandModel>>>();
            _query = new Mock<IUserQuery>();
            _jwtHelp = new Mock<IJwtHelper>();
        }

        [Fact]
        public async Task UserSignIn()
        {
            _query
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<UserQueryModel> {
                new UserQueryModel{
                    Account = "testAccount",
                    Password = "ZvczkadhFgUUPBY3r5N5f5RGFvCb4s3ptNzB3OoQWLpN5iBe5uPUrMvDsIR7hsUPSJktB2mvta+YzxICKYQO0Q==",
                    Salt = "*lZzd8@KB8*[/v]"
                }});

            _jwtHelp
            .Setup(x => x.GenerateToken(It.IsAny<string?>()))
            .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdHJpbmciLCJqdGkiOiJjM2FiYzgyYy03ZGU1LTQ5ODgtYjRmNy1kZmIxZDNlNGU0YjMiLCJuYmYiOjE2Nzc3MjU0MjgsImV4cCI6MTY3NzcyNzIyOCwiaWF0IjoxNjc3NzI1NDI4LCJpc3MiOiJKd3RBdXRoIn0.fWcFRo7G5q7Ro5imY-QOtdJvL1_8EcNOuFV_HA-QbAo");

            _handler = new UserManageCommandHandler(
               _insertMock.Object,
               _updateMock.Object,
               _query.Object,
               _jwtHelp.Object);

            var rsp = await _handler.HandleAsync(new ReqUserSignIn
            {
                Password = "testpwd605",
                Account = "testAccount",
            });

            rsp.Token.Should().NotBeNullOrEmpty();

            _jwtHelp.Verify(x => x.GenerateToken(It.IsAny<string?>()), Times.Once());
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task UserManageCreate()
        {
            _handler = new UserManageCommandHandler(
                _insertMock.Object,
                _updateMock.Object,
                _query.Object,
                _jwtHelp.Object);

            _query
                .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<UserQueryModel> { });

            _insertMock.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<UserCommandModel>>()));

            await _handler.HandleAsync(new ReqUserCreate
            {
                Name = "LIN",
                Email = "test@mail.com",
                Password = "testpwd605",
                Account = "testAccount",
            });
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
            _insertMock.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<UserCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task UserUpdate()
        {
            _handler = new UserManageCommandHandler(
                _insertMock.Object,
                _updateMock.Object,
                _query.Object,
                _jwtHelp.Object);
            _query
                .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<UserQueryModel> { new UserQueryModel() });

            _updateMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<UserCommandModel>>()));

            await _handler.HandleAsync(new ReqUserUpdate
            {
                Id = 1,
                Name = "LIN",
                Email = "test@mail.com",
            });
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
            _updateMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<UserCommandModel>>()), Times.Once());
        }
    }
}
