namespace BudgetPlanner.Application.Interfaces;

public interface IExpenseRepository
{
    public Task<Expense> GetOneExpenseAsync(int id);
    public Task<List<Expense>> GetExpensesAsync();
    public Task<int> SaveExpenseAsync(Expense item);
    public Task<int> DeleteExpenseAsync(int id);
}
