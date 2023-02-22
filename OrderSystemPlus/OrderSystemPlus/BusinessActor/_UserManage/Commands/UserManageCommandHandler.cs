using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Commands;

namespace OrderSystemPlus.BusinessActor.Commands
{
    public class UserManageCommandHandler : ICommandHandler<ReqUserManageCreate>
    {
        IInsertCommand<IEnumerable<UserCommandModel>> _insertCommand;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="insertCommand"></param>
        public UserManageCommandHandler(
            IInsertCommand<IEnumerable<UserCommandModel>> insertCommand)
        {
            _insertCommand = insertCommand;
        }

        /// <summary>
        /// 建立使用者
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqUserManageCreate req)
        {
            var now = DateTime.Now;
            await _insertCommand.InsertAsync(new List<UserCommandModel>
            {
                new UserCommandModel
                {
                    Name = req.Name,
                    Salt  = "TODO",
                    Email = req.Email,
                    Account  = req.Account,
                    Password = req.Password,
                    RoleId  = 1,
                    IsValid = true,
                    CreatedOn = now,
                    UpdatedOn = now,
                }
            });
        }
    }
}
