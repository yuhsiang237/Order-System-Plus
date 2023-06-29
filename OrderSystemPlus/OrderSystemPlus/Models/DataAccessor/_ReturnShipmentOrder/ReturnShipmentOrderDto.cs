namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ReturnShipmentOrderDto
    /// </summary>
    public class ReturnShipmentOrderDto
    {
        public string ReturnShipmentOrderNumber { get; set; }
        public string ShipmentOrderNumber { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Remark { get; set; }
        public decimal? TotalReturnAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsValid { get; set; }
        public List<ReturnShipmentOrderDetailDto> Details { get; set; }
    }
}
