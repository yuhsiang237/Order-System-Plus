﻿using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IReturnShipmentOrderRepository
    {
        /// <summary>
        /// 更新ReturnShipmentOrder資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<ReturnShipmentOrderDto> model);

        /// <summary>
        /// 刪除ReturnShipmentOrder資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<ReturnShipmentOrderDto> model);

        /// <summary>
        /// 新增ReturnShipmentOrder資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<string>> InsertAsync(IEnumerable<ReturnShipmentOrderDto> model);

        /// <summary>
        /// 查詢ReturnShipmentOrder資料們
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        Task<List<ReturnShipmentOrderDto>> FindByOptionsAsync(string? returnShipmentOrderNumber = null);
    }
}
