﻿using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IProductTypeManageHandler
    {
        Task HandleAsync(ReqCreateProductType req);
        Task HandleAsync(ReqUpdateProductType req);
        Task HandleAsync(List<ReqDeleteProductType> req);
        Task<RspGetProductTypeList> GetProductTypeListAsync(ReqGetProductTypeList req);
    }
}
