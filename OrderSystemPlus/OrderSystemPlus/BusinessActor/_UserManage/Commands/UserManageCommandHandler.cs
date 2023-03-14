using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.Utils.HashSaltTool;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlus.BusinessActor.Commands
{
    public class UserManageCommandHandler :
        ICommandHandler<ReqCreateUser>,
        ICommandHandler<ReqSignInUser, RspSignInUser>,
        ICommandHandler<ReqUpdateUser>
    {
        private readonly IInsertCommand<IEnumerable<UserCommandModel>> _insert;
        private readonly IUpdateCommand<IEnumerable<UserCommandModel>> _update;
        private readonly IUserQuery _query;
        private readonly IJwtHelper _jwtHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="insertCommand"></param>
        /// <param name="query"></param>
        public UserManageCommandHandler(
            IInsertCommand<IEnumerable<UserCommandModel>> insert,
            IUpdateCommand<IEnumerable<UserCommandModel>> update,
            IUserQuery query,
            IJwtHelper jwtHelper)
        {
            _insert = insert;
            _update = update;
            _query = query;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// 建立使用者
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqCreateUser req)
        {
            var now = DateTime.Now;
            var hashSalt = HashSaltTool.Generate(req.Password);
            var isExist = (await _query.FindByOptionsAsync(null, null, req.Account))
                        .ToList()
                        .Any();
            if (!isExist)
            {
                await _insert.InsertAsync(new List<UserCommandModel>
                {
                    new UserCommandModel
                    {
                        Name = req.Name,
                        Salt  = hashSalt.Salt,
                        Email = req.Email,
                        Account  = req.Account,
                        Password = hashSalt.Hash,
                        RoleId  = (int)RoleType.Basic,
                        IsValid = true,
                        CreatedOn = now,
                        UpdatedOn = now,
                    }
                });
            }
            else
            {
                throw new Exception("使用者已存在");
            }
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<RspSignInUser> HandleAsync(ReqSignInUser command)
        {
            var user = (await _query.FindByOptionsAsync(null, null, command.Account)).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var isValid = HashSaltTool.Validate(command.Password,
                                                    user.Salt,
                                                    user.Password);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(ReqUpdateUser command)
        {
            var hasUser = (await _query.FindByOptionsAsync(command.Id, null, null))
                .Any();
            if (hasUser)
            {
                await _update.UpdateAsync(new List<UserCommandModel>
                {
                    new UserCommandModel
                    {
                        Id = command.Id,
                        Name = command.Name,
                        Email = command.Email,
                        UpdatedOn = DateTime.Now,
                    }
                });
            }
        }
    }
}
