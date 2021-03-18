namespace TRMediator.Core.Abstractions
{
    public interface ICommand
    {
    }
    
    public interface ICommand<out TResponse>
    {
    }
}