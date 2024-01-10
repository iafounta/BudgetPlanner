namespace BudgetPlanner.Application.UseCases.Income;

public class UpdateIncome : ICommand<Result<Unit>>
{
    public int Id { get; }
    public string Name { get; }
    public float Amount { get; }
    public string TimeInterval { get; }

    public UpdateIncome(int id, string name, float amount, string timeInterval)
    {
        Id = id;
        Name = name;
        Amount = amount;
        TimeInterval = timeInterval;
    }

    public class UpdateIncomesHandler : ICommandHandler<UpdateIncome, Result<Unit>>
    {
        private readonly IIncomeRepository _repository;

        public UpdateIncomesHandler(IIncomeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(UpdateIncome request, CancellationToken cancellationToken)
        {
            Domain.Entities.Income Income = await _repository.GetOneIncomeAsync(request.Id);

            if (Income is null)
            {
                return Result<Unit>.Failure(ErrorMessage.CannotFetchIncome);
            }

            var result = await _repository.SaveIncomeAsync(Income);

            if (result == 0)
            {
                return Result<Unit>.Failure(ErrorMessage.CannotSaveIncome);
            }

            return Result<Unit>.Success();
        }
    }
}
