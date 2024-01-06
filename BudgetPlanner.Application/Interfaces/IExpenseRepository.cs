namespace BudgetPlanner.Application.Interfaces;

public interface IExpenseRepository
{
    public Task<Result<Unit>> SaveExpense(Expense expenses);
    public Task<Result<IEnumerable<Expense>>> GetAllExpenses();
}
