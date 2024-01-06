namespace BudgetPlanner.Domain.Results;

public static class ErrorMessage
{
    public static readonly Error CannotCreateExpenses =
        Error.Create("CannotCreateExpenses", "Expenses cannot be created.");
}
