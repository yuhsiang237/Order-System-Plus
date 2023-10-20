using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IShipmentOrderRepository
    {
        /// <summary>
        /// 更新ShipmentOrder資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<ShipmentOrderDto> model);

        /// <summary>
        /// 刪除ShipmentOrder資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<ShipmentOrderDto> model);

        /// <summary>
        /// 新增ShipmentOrder資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<string>> InsertAsync(IEnumerable<ShipmentOrderDto> model);

        /// <summary>
        /// 查詢ShipmentOrder資料們
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        Task<(int TotalCount, List<ShipmentOrderDto> Data)> FindByOptionsAsync(
            string? orderNumber = null,
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null);
    }
}
