namespace BudgetPlanner.Application.UseCases.Income;

public class DeleteIncome : ICommand<Result<Unit>>
{
    public int Id { get; set; }

    public DeleteIncome(int Id)
    {
        this.Id = Id;
    }

    public class DeleteIncomeHandler : ICommandHandler<DeleteIncome, Result<Unit>>
    {
        private readonly IIncomeRepository _repository;

        public DeleteIncomeHandler(IIncomeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteIncome request, CancellationToken cancellationToken)
        {

            var result = await _repository.DeleteIncomeAsync(request.Id);

            if (result == 0)
            {
                return Result<Unit>.Failure(ErrorMessage.CannotDeleteIncome);
            }

            return Result<Unit>.Success();
        }
    }
}
