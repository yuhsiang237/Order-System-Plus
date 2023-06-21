﻿namespace OrderSystemPlus.Models.BusinessActor
{
    /// <summary>
    /// RspGetShipmentOrderList
    /// </summary>
    public class RspGetShipmentOrderList
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
    }
}
