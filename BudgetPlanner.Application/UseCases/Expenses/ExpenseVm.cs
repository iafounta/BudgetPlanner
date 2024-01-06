namespace BudgetPlanner.Application.UseCases.Expenses;

public class ExpenseVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Amount { get; set; }
    public string TimeInterval { get; set; } = string.Empty;
}
