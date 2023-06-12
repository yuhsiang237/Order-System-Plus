using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentAssertions;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Utils.GUIDSerialTool;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlusTest.DataAccessor
{
    public class UserRepositoryDataAccessorTest
    {
        private IUserRepository _repository;
        private DateTime _now;
        private string _guid;

        public UserRepositoryDataAccessorTest()
        {
            _now = DateTime.Now;
            _guid = GUIDSerialTool.Generate(15);
            _repository = new UserRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.InsertAsync(GetInsertModel());
            var insertResult = await _repository.FindByOptionsAsync(
                email: GetInsertModel().Email,
                account: GetInsertModel().Account);
            insertResult.Count.Should().Be(1);

            await _repository.UpdateAsync(new List<UserDto> { GetUpdateModel(insertResult.First().Id) });
            var updateResult = await _repository.FindByOptionsAsync(insertResult.First().Id, null, null);
            updateResult.Count.Should().Be(1);
            updateResult.First().Name.Should().Be("UpdateTest");

            await _repository.DeleteAsync(new List<UserDto> { GetDeleteModel(insertResult.First().Id) });
            var deleteResult = await _repository.FindByOptionsAsync(id: insertResult.First().Id);
            deleteResult.Count.Should().Be(0);
        }

        public UserDto GetInsertModel() =>
                new UserDto
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
                };

        public UserDto GetUpdateModel(int id) => new UserDto
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
        };

        public UserDto GetRefreshModel(int id) => new UserDto
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
        };

        public UserDto GetDeleteModel(int id) => new UserDto
        {
            Id = id,
            UpdatedOn = _now,
        };
    }
}
