using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.DataAccessor;

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
        Task<(int TotalCount, List<ReturnShipmentOrderDto> Data)> FindByOptionsAsync(
            string? returnShipmentOrderNumber = null,
            string? shipmentOrderNumber = null,
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null);
    }
}
