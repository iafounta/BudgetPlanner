namespace BudgetPlanner.Application.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
