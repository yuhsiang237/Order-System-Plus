namespace OrderSystemPlus.BusinessActor
{
    public interface ICommandHandler<TCommand>
    {
        /// <summary>
        /// HandleAsync
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>Task</returns>
        Task HandleAsync(TCommand command);
    }
}
