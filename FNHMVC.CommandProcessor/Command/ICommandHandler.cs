namespace FNHMVC.CommandProcessor.Command
{
    public interface ICommandHandler<in TCommand>
    {
        ICommandResult Execute(TCommand command);
    }
}

