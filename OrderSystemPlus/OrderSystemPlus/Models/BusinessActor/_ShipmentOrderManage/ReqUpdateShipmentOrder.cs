namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// ReqUpdateShipmentOrder
    /// </summary>
    public class ReqUpdateShipmentOrder
    {
        public string OrderNumber { get; set; }
        public string? RecipientName { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public List<DetailModel> Details { get; set; }
        public class DetailModel
        {
            public int Id { get; set; }
            public string Remarks { get; set; }
        }
    }
}
