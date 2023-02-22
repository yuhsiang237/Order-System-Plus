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
        private IUpdateCommand<IEnumerable<UserCommandModel>> _update;
        private DateTime _now;
        private string _guid;

        public UserQueryDataAccessorTest()
        {
            _now = DateTime.Now;
            _guid = Guid.NewGuid().ToString("N")[..15];
            _query = new UserQuery();
            _insert = new UserCommand();
            _delete = new UserCommand();
            _refresh = new UserCommand();
            _update = new UserCommand();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _insert.InsertAsync(GetInsertModel());
            var insertResult = await _query.FindByOptionsAsync(null, GetInsertModel().First().Email, GetInsertModel().First().Account);
            insertResult.Count.Should().Be(1);

            await _update.UpdateAsync(GetUpdateModel(insertResult.First().Id));
            var updateResult = await _query.FindByOptionsAsync(insertResult.First().Id, null, null);
            updateResult.Count.Should().Be(1);
            updateResult.First().Name.Should().Be("UpdateTest");

            await _refresh.RefreshAsync(GetRefreshModel(updateResult.First().Id));
            var refreshResult = await _query.FindByOptionsAsync(null, null, updateResult.First().Account);
            refreshResult.Count.Should().Be(1);
            refreshResult.First().Name.Should().Be("RefreshTest");

            await _delete.DeleteAsync(GetDeleteModel(refreshResult.First().Id));
            var deleteResult = await _query.FindByOptionsAsync(refreshResult.First().Id, null, null);
            deleteResult.Count.Should().Be(0);
        }

        public List<UserCommandModel> GetInsertModel() => new List<UserCommandModel> {
                new UserCommandModel
                {
                    Name = "Test",
                    Password = "TEST",
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                    Account = $"Test{_guid}",
                    RoleId = 1,
                    Salt = "SALT",
                    Email = "test@gmail.com"
                }
            };

        public List<UserCommandModel> GetUpdateModel(int id) => new List<UserCommandModel> {
                new UserCommandModel
                {
                    Id = id,
                    Name = "UpdateTest",
                    Password = "TEST",
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                    Account = $"Test{_guid}",
                    RoleId = 1,
                    Salt = "SALT",
                    Email = "test@gmail.com"
                }
            };

        public List<UserCommandModel> GetRefreshModel(int id) => new List<UserCommandModel> {
                new UserCommandModel
                {
                    Id = id,
                    Name = "RefreshTest",
                    Password = "TEST",
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                    Account = $"Test{_guid}",
                    RoleId = 1,
                    Salt = "SALT",
                    Email = "test@gmail.com"
                }
            };

        public List<UserCommandModel> GetDeleteModel(int id) => new List<UserCommandModel> {
                 new UserCommandModel
                {
                    Id = id,
                    UpdatedOn = _now,
                }
            };
    }
}
