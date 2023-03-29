namespace OrderSystemPlus.Models.DataAccessor.Queries
{
    /// <summary>
    /// ProductProductTypeRelationshipQueryModel
    /// </summary>
    public class ProductProductTypeRelationshipQueryModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 產品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 類別ID
        /// </summary>
        public int ProductTypeId { get; set; }
    }
}
