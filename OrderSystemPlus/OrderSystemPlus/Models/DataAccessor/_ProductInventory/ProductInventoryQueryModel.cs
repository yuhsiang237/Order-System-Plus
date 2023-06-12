﻿using OrderSystemPlus.Enums;

namespace OrderSystemPlus.Models.DataAccessor
{
    /// <summary>
    /// ProductInventoryQueryModel
    /// </summary>
    public class ProductInventoryQueryModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ActionType
        /// </summary>
        public InventoryActionType ActionType { get; set; }
        /// <summary>
        /// 產品庫存量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }
    }
}