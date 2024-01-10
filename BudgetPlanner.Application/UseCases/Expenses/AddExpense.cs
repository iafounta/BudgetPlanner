
using BudgetPlanner.Domain.Results;

namespace BudgetPlanner.Application.UseCases.Expenses;

public class AddExpense : ICommand<Result<int>>
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

    public class AddExpensesHandler : ICommandHandler<AddExpense, Result<int>>
    {
        private readonly IExpenseRepository _repository;

        public AddExpensesHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> Handle(AddExpense request, CancellationToken cancellationToken)
        {
            Domain.Entities.Expense expense = new()
            {
                Name = request.Name,
                Amount = request.Amount,
                TimeInterval = request.TimeInterval
            };

            int id = await _repository.SaveExpenseAsync(expense);

            if (id == 0)
            {
                return Result<int>.Failure(ErrorMessage.CannotSaveExpenses);
            }

            return Result<int>.Success(id);
        }
    }
}
