namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ShipmentOrderDto
    /// </summary>
    public class ShipmentOrderDto
    {
        public string OrderNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? RecipientName { get; set; }
        public int? OperatorUserId { get; set; }
        public int? Status { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public string Address { get; set; }
        public string Remark { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsValid { get; set; }
        public List<ShipmentOrderDetailDto> Details { get; set; }
    }
}
