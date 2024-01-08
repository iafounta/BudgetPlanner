
namespace BudgetPlanner.Application.UseCases.Expenses;

public class GetAllExpense : IQuery<Result<List<ExpenseVm>>>
{
    public class GetAllExpenseHandler : IQueryHandler<GetAllExpense, Result<List<ExpenseVm>>>
    {
        private readonly IExpenseRepository _repository;
        private readonly IMapper<Expense, ExpenseVm> _mapper;

        public GetAllExpenseHandler(IExpenseRepository repository, IMapper<Expense, ExpenseVm> mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<ExpenseVm>>> Handle(GetAllExpense request, CancellationToken cancellationToken)
        {
            List<Expense> allExpenses = await _repository.GetExpensesAsync();

            if (allExpenses == null)
            {
                return Result<List<ExpenseVm>>.Failure(ErrorMessage.CannotFetchExpenses);
            }

            IEnumerable<ExpenseVm> mappedExpenses = _mapper.Map(allExpenses);

            return Result<List<ExpenseVm>>.Success(mappedExpenses.ToList());
        }
    }
}
