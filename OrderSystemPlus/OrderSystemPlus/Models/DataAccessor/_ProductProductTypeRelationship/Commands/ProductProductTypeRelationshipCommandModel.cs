namespace OrderSystemPlus.Models.DataAccessor.Commands
{
    /// <summary>
    /// ProductProductTypeRelationshipCommandModel
    /// </summary>
    public class ProductProductTypeRelationshipCommandModel
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
