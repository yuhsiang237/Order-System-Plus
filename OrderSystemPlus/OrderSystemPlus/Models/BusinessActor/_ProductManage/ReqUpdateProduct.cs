﻿namespace OrderSystemPlus.Models.BusinessActor
{
    public class ReqUpdateProduct
    {
        public int Id { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 產品類別Ids
        /// </summary>
        public List<int?> ProductTypeIds { get; set; }
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
    }
}
