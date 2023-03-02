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

    public interface ICommandHandler<TCommand,TResponse>
    {
        /// <summary>
        /// HandleAsync
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>Task<TResponse></returns>
        Task<TResponse> HandleAsync(TCommand command);
    }
}
