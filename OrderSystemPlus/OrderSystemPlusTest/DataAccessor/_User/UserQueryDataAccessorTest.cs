using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentAssertions;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.DataAccessor.Commands;

namespace OrderSystemPlusTest.DataAccessor
{
    public class UserQueryDataAccessorTest
    {
        private IUserQuery _query;
        private IInsertCommand<IEnumerable<UserCommandModel>> _insert;
        private IDeleteCommand<IEnumerable<UserCommandModel>> _delete;
        private IRefreshCommand<IEnumerable<UserCommandModel>> _refresh;

        public UserQueryDataAccessorTest()
        {
            _query = new UserQuery();
            _insert = new UserCommand();
            _delete = new UserCommand();
            _refresh = new UserCommand();
        }

        [Fact]
        public async Task RunAsync()
        {
            var now = DateTime.Now;
            var guid = Guid.NewGuid().ToString("N")[..15];
            await _insert.InsertAsync(new List<UserCommandModel> {
                new UserCommandModel {
                    Name = "Test",
                    Password = "TEST",
                    IsValid = true,
                    CreatedOn = now,
                    UpdatedOn = now,
                    Account = $"Test{guid}",
                    RoleId = 1,
                    Salt = "SALT",
                    Email = "test@gmail.com"
                }
            });

            var rsp1 = await _query.FindByOptionsAsync(null, "test@gmail.com", $"Test{guid}");
            rsp1.Count.Should().Be(1);

            await _refresh.RefreshAsync(new List<UserCommandModel>
            {
                new UserCommandModel {
                    Id = rsp1.First().Id,
                    Name = "Test",
                    Password = "TEST",
                    IsValid = true,
                    CreatedOn = now,
                    UpdatedOn = now,
                    Account = $"Test{guid}F",
                    RoleId = 1,
                    Salt = "SALT",
                    Email = "test@gmail.com"
                }
            });

            var rsp2 = await _query.FindByOptionsAsync(null, "test@gmail.com", $"Test{guid}F");
            rsp2.Count.Should().Be(1);
            rsp2.First().Account.Should().Be($"Test{guid}F");

            await _delete.DeleteAsync(new List<UserCommandModel>
            {
                new UserCommandModel
                {
                    Id = rsp2.First().Id,
                    UpdatedOn = now,
                }
            });
        }
    }
}
