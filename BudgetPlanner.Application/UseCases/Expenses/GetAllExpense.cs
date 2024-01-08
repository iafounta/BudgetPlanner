
namespace BudgetPlanner.Application.UseCases.Expenses;

public class GetAllExpense : IQuery<Result<List<Expense>>>
{
    public class GetAllExpenseHandler : IQueryHandler<GetAllExpense, Result<List<Expense>>>
    {
        private readonly IExpenseRepository _repository;

        public GetAllExpenseHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Expense>>> Handle(GetAllExpense request, CancellationToken cancellationToken)
        {
            List<Expense> allExpenses = await _repository.GetExpensesAsync();

            if (allExpenses == null)
            {
                return Result<List<Expense>>.Failure(ErrorMessage.CannotFetchExpenses);
            }

            return Result<List<Expense>>.Success(allExpenses.ToList());
        }
    }
}
