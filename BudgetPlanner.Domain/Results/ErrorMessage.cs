namespace BudgetPlanner.Domain.Results;

public static class ErrorMessage
{

    public static readonly Error CannotSaveExpenses =
    Error.Create("CannotSaveExpenses", "Expenses cannot be saved.");

    public static readonly Error CannotDeleteExpenses =
    Error.Create("CannotDeleteExpenses", "Expenses cannot be deteted.");

    public static readonly Error CannotFetchExpenses =
        Error.Create("CannotFetchExpenses", "Expenses cannot be fetched.");


    public static readonly Error CannotSaveIncome =
Error.Create("CannotSaveIncome", "Income cannot be saved.");

    public static readonly Error CannotDeleteIncome =
    Error.Create("CannotDeleteIncome", "Income cannot be deteted.");

    public static readonly Error CannotFetchIncome =
        Error.Create("CannotFetchIncomes", "Incomes cannot be fetched.");
}
