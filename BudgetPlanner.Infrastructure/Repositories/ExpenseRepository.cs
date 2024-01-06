namespace BudgetPlanner.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    public async Task<Result<IEnumerable<Expense>>> GetAllExpenses()
    {
        IEnumerable<Expense> allExpense = new List<Expense>() {
            new Expense() { Name = "Miete", Amount = 1500.00f,  TimeInterval = "Monatlich"},
            new Expense() { Name = "Krankenkasse", Amount = 320.50f,  TimeInterval = "Monatlich"},
            new Expense() { Name = "Internet", Amount = 80.00f,  TimeInterval = "Monatlich"},
            new Expense() { Name = "Einkaufen", Amount = 100.00f,  TimeInterval = "Wochenlich"}
         };

        await Task.CompletedTask;

        return Result<IEnumerable<Expense>>.Success(allExpense);
    }

    public async Task<Result<Unit>> SaveExpense(Expense expense)
    {
        await Task.CompletedTask;

        return Result<Unit>.Success();
    }
}
