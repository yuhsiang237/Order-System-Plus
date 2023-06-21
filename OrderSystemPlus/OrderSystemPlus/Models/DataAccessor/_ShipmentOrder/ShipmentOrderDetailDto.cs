namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ShipmentOrderDetailDto
    /// </summary>
    public class ShipmentOrderDetailDto
    {
        public string OrderNumber { get; set; }
        public int? ProductId { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ProductQuantity { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsValid { get; set; }
    }
}
