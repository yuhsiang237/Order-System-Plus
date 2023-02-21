namespace OrderSystemPlus.DataAccessor
{
    public interface IInsertCommand<TCommand>
    {
        /// <summary>
        /// InsertAsync
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>Task</returns>
        Task InsertAsync(TCommand command);
    }
}
