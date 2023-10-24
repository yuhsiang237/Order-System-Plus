using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// ReqGetReturnShipmentOrderList
    /// </summary>
    public class ReqGetReturnShipmentOrderList
    {
        public string? ReturnShipmentOrderNumber { get; set; }
        public string? ShipmentOrderNumber { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SortField { get; set; }
        public SortType? SortType { get; set; }
    }
}
