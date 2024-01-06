
namespace BudgetPlanner.Application.UseCases.Expenses;

public class AddExpense : ICommand<Result<Unit>>
{
    public string Name { get; }
    public float Amount { get; }
    public string TimeInterval { get; }

    public AddExpense(string name, float amount, string timeInterval)
    {
        Name = name;
        Amount = amount;
        TimeInterval = timeInterval;
    }

    public class AddExpensesHandler : ICommandHandler<AddExpense, Result<Unit>>
    {
        private readonly IExpenseRepository _repository;

        public AddExpensesHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(AddExpense request, CancellationToken cancellationToken)
        {
            Domain.Entities.Expense expense = new()
            {
                Name = request.Name,
                Amount = request.Amount,
                TimeInterval = request.TimeInterval
            };

            var result = await _repository.SaveExpense(expense);

            if (!result.IsSuccess)
            {
                return Result<Unit>.Failure(result.Error);
            }

            return Result<Unit>.Success();
        }
    }
}
