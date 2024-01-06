namespace BudgetPlanner.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public float Amount { get; set; }
    public string TimeInterval { get; set; } = string.Empty;
}
