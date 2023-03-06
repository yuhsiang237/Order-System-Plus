using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.DataAccessor.Queries;
using OrderSystemPlus.BusinessActor.Queries;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlusTest.BusinessActor.Queries
{
    public class UserManageQueryHandlerTest
    {
        private IUserManageQueryHandler _handler;
        private readonly Mock<IUserQuery> _query;

        public UserManageQueryHandlerTest()
        {
            _query = new Mock<IUserQuery>();
        }

        [Fact]
        public async Task GetUserListAsync()
        {
            _query
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<UserQueryModel> {
                new UserQueryModel{
                    Account = "testAccount",
                }});
            _handler = new UserManageQueryHandler(
               _query.Object);
            var rsp = await _handler.GetUserListAsync(
            new ReqGetUserList
            {

            });
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task GetUserInfo()
        {
            _query
           .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
           .ReturnsAsync(new List<UserQueryModel> {
                new UserQueryModel{
                    Account = "testAccount",
                }});
            _handler = new UserManageQueryHandler(
               _query.Object);
            var rsp = await _handler.GetUserInfoAsync(
            new ReqGetUserInfo
            {

            });
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }
    }
}
