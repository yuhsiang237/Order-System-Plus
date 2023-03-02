namespace OrderSystemPlus.DataAccessor
{
    public interface IRefreshCommand<TCommand>
    {
        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>Task</returns>
        Task RefreshAsync(TCommand command);
    }
}
