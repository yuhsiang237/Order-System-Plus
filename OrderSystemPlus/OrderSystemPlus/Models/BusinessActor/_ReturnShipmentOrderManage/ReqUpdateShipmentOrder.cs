﻿namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// ReqUpdateReturnShipmentOrder
    /// </summary>
    public class ReqUpdateReturnShipmentOrder
    {
        public string ReturnShipmentOrderNumber { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Remark { get; set; }
        public List<ReturnShipmentOrderDetail> Details { get; set; }
        public class ReturnShipmentOrderDetail
        {
            public int Id { get; set; }
            public string ReturnShipmentOrderNumber { get; set; }
            public int? ShipmentOrderDetailId { get; set; }
            public decimal? ReturnProductQuantity { get; set; }
            public string Remarks { get; set; }
        }
    }
}
