using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// ReqGetShipmentOrderList
    /// </summary>
    public class ReqGetShipmentOrderList
    {
        public string OrderNumber { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SortField { get; set; }
        public SortType? SortType { get; set; }
    }
}
