namespace OrderSystemPlus.DataAccessor
{
    public interface IUpdateCommand<TCommand>
    {
        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>Task</returns>
        Task UpdateAsync(TCommand command);
    }
}
