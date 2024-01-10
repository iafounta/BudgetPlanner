namespace BudgetPlanner.Application.UseCases.Income;

public class AddIncome : ICommand<Result<int>>
{
    public string Name { get; }
    public float Amount { get; }
    public string TimeInterval { get; }

    public AddIncome(string name, float amount, string timeInterval)
    {
        Name = name;
        Amount = amount;
        TimeInterval = timeInterval;
    }

    public class AddIncomeHandler : ICommandHandler<AddIncome, Result<int>>
    {
        private readonly IIncomeRepository _repository;

        public AddIncomeHandler(IIncomeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> Handle(AddIncome request, CancellationToken cancellationToken)
        {
            Domain.Entities.Income Income = new()
            {
                Name = request.Name,
                Amount = request.Amount,
                TimeInterval = request.TimeInterval
            };

            int id = await _repository.SaveIncomeAsync(Income);

            if (id == 0)
            {
                return Result<int>.Failure(ErrorMessage.CannotSaveIncome);
            }

            return Result<int>.Success(id);
        }
    }
}
