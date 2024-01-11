namespace BudgetPlanner.Application.UseCases.Income;

public class GetAllIncomes : IQuery<Result<List<Domain.Entities.Income>>>
{
    public class GetAllIncomeHandler : IQueryHandler<GetAllIncomes, Result<List<Domain.Entities.Income>>>
    {
        private readonly IIncomeRepository _repository;

        public GetAllIncomeHandler(IIncomeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Domain.Entities.Income>>> Handle(GetAllIncomes request, CancellationToken cancellationToken)
        {
            List<Domain.Entities.Income> allIncomes = await _repository.GetIncomesAsync();

            if (allIncomes == null)
            {
                return Result<List<Domain.Entities.Income>>.Failure(ErrorMessage.CannotFetchIncome);
            }

            return Result<List<Domain.Entities.Income>>.Success(allIncomes.ToList());
        }
    }
}
