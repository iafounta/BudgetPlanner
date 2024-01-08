namespace BudgetPlanner.Domain.Results;

public static class ErrorMessage
{

    public static readonly Error CannotSaveExpenses =
    Error.Create("CannotSaveExpenses", "Expenses cannot be saved.");

    public static readonly Error CannotDeleteExpenses =
    Error.Create("CannotDeleteExpenses", "Expenses cannot be deteted.");

    public static readonly Error CannotFetchExpenses =
        Error.Create("CannotFetchExpenses", "Expenses cannot be fetched.");
}
