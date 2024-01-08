namespace BudgetPlanner.Application.UseCases.Expenses;

public class UpdateExpense : ICommand<Result<Unit>>
{
    public Guid Id { get; }
    public string Name { get; }
    public float Amount { get; }
    public string TimeInterval { get; }

    public UpdateExpense(Guid id, string name, float amount, string timeInterval)
    {
        Id = id;
        Name = name;
        Amount = amount;
        TimeInterval = timeInterval;
    }

    public class UpdateExpensesHandler : ICommandHandler<UpdateExpense, Result<Unit>>
    {
        private readonly IExpenseRepository _repository;

        public UpdateExpensesHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(UpdateExpense request, CancellationToken cancellationToken)
        {
            Expense expense = await _repository.GetOneExpenseAsync(request.Id);

            if (expense is null)
            {
                return Result<Unit>.Failure(ErrorMessage.CannotFetchExpenses);
            }

            var result = await _repository.SaveExpenseAsync(expense);

            if (result == 0)
            {
                return Result<Unit>.Failure(ErrorMessage.CannotSaveExpenses);
            }

            return Result<Unit>.Success();
        }
    }
}
