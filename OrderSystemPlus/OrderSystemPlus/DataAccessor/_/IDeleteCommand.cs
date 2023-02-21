namespace OrderSystemPlus.DataAccessor
{
    public interface IDeleteCommand<TCommand>
    {
        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>Task</returns>
        Task DeleteAsync(TCommand command);
    }
}
