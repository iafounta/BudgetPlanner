
using BudgetPlanner.Domain.Results;

namespace BudgetPlanner.Application.UseCases.Expenses;

public class AddExpense : ICommand<Result<Guid>>
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

    public class AddExpensesHandler : ICommandHandler<AddExpense, Result<Guid>>
    {
        private readonly IExpenseRepository _repository;

        public AddExpensesHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid>> Handle(AddExpense request, CancellationToken cancellationToken)
        {
            Domain.Entities.Expense expense = new()
            {
                Name = request.Name,
                Amount = request.Amount,
                TimeInterval = request.TimeInterval
            };

            var result = await _repository.SaveExpenseAsync(expense);

            if (result == 0)
            {
                return Result<Guid>.Failure(ErrorMessage.CannotSaveExpenses);
            }

            return Result<Guid>.Success(expense.Id);
        }
    }
}
