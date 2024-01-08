namespace BudgetPlanner.Application.UseCases.Expenses;

public class DeleteExpense : ICommand<Result<Unit>>
{
    public Guid Id { get; set; }

    public DeleteExpense(Guid Id)
    {
        this.Id = Id;
    }

    public class DeleteExpensesHandler : ICommandHandler<DeleteExpense, Result<Unit>>
    {
        private readonly IExpenseRepository _repository;

        public DeleteExpensesHandler(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteExpense request, CancellationToken cancellationToken)
        {

            var result = await _repository.DeleteExpenseAsync(request.Id);

            if (result == 0)
            {
                return Result<Unit>.Failure(ErrorMessage.CannotDeleteExpenses);
            }

            return Result<Unit>.Success();
        }
    }
}
