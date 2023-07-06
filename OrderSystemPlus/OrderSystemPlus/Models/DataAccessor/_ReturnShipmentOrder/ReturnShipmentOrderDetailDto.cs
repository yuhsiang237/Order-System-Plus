namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ReturnShipmentOrderDetailDto
    /// </summary>
    public class ReturnShipmentOrderDetailDto
    {
        public int Id { get; set; }
        public string ReturnShipmentOrderNumber { get; set; }
        public int ShipmentOrderDetailId { get; set; }
        public decimal? ReturnProductQuantity { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsValid { get; set; }
    }
}
