﻿using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.HashSaltTool;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlus.BusinessActor
{
    public class UserManageHandler : IUserManageHandler
    {
        private readonly IUserRepository _userRepository;
        private static SemaphoreSlim _userCreateSemaphoreSlim;

        public UserManageHandler(
            IUserRepository userRepository)
        {
            _userCreateSemaphoreSlim = new SemaphoreSlim(1, 1);
            _userRepository = userRepository;
        }

        public async Task<RspGetUserList> GetUserListAsync(ReqGetUserList req)
        {
            var data = await _userRepository.FindByOptionsAsync(
              account:req.Account,
              pageIndex: req.PageIndex,
              pageSize: req.PageSize,
              sortField: req.SortField,
              sortType: req.SortType
              );
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, RspGetUserListItem>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<UserDto>, List<RspGetUserListItem>>(data.Data);
            return new RspGetUserList
            {
                Data = rsp,
                TotalCount = data.TotalCount,
            };
        }

        public async Task<RspGetUserInfo> GetUserInfoAsync(ReqGetUserInfo req)
        {
            var data = (await _userRepository.FindByOptionsAsync(id: req.Id)).Data.FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, RspGetUserInfo>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<UserDto, RspGetUserInfo>(data);
            return rsp;
        }

        public async Task<int> HandleAsync(ReqCreateUser req)
        {
            await _userCreateSemaphoreSlim.WaitAsync();
            try
            {
                var result = default(int);
                var now = DateTime.Now;
                var hashSalt = HashSaltTool.Generate(req.Password);
                var isExist = (await _userRepository.FindByOptionsAsync(account: req.Account))
                            .Data.ToList()
                            .Any();
                if (isExist)
                    throw new BusinessException("使用者已存在");

                result = await _userRepository.InsertAsync(
                        new UserDto
                        {
                            Name = req.Name,
                            Salt = hashSalt.Salt,
                            Email = req.Email,
                            Account = req.Account,
                            Password = hashSalt.Hash,
                            RoleId = (int)RoleType.Basic,
                            IsValid = true,
                            CreatedOn = now,
                            UpdatedOn = now,
                        });

                return await Task.FromResult(result);
            }
            finally
            {
                _userCreateSemaphoreSlim.Release();
            }
        }

        public async Task HandleAsync(ReqUpdateUser req)
        {
            var now = DateTime.Now;
            var dto = new UserDto
            {
                Id = req.Id,
                Name = req.Name,
                Email = req.Email,
                UpdatedOn = now,
            };
            await _userRepository.UpdateAsync(
                new List<UserDto> { dto });
        }

        public async Task HandleAsync(ReqDeleteUser req)
        {
            var now = DateTime.Now;
            var dto = new UserDto
            {
                Id = req.Id,
                UpdatedOn = now,
            };
            await _userRepository.DeleteAsync(
                new List<UserDto> { dto });
        }
    }
}
