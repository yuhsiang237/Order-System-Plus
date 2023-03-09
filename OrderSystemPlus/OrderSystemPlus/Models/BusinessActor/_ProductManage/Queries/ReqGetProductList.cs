﻿namespace OrderSystemPlus.Models.BusinessActor.Queries
{
    public class ReqGetProductList
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 產品價格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 產品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 產品目前數量
        /// </summary>
        public decimal CurrentUnit { get; set; }
    }
}
