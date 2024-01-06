
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
            var allExpenses = await _repository.GetAllExpenses();

            if (!allExpenses.IsSuccess)
            {
                return Result<List<ExpenseVm>>.Failure(allExpenses.Error);
            }

            var mappedExpenses = _mapper.Map(allExpenses.Value);

            return Result<List<ExpenseVm>>.Success(mappedExpenses.ToList());
        }
    }
}
