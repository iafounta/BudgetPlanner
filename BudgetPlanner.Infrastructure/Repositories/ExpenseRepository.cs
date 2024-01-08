using BudgetPlanner.Application.UseCases.Expenses;
using BudgetPlanner.Infrastructure.Interfaces;
using SQLite;


namespace BudgetPlanner.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly string databasePath;
    private readonly SQLiteAsyncConnection database;
    private const string DatabaseFilename = "ExpensesSQLite.db3";
    private const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

    public ExpenseRepository(IDatabasePathProvider pathProvider)
    {
        databasePath = pathProvider.GetDatabasePath();
        database = new SQLiteAsyncConnection(databasePath, Flags);
        database.CreateTableAsync<Expense>().Wait();
    }
    public Task<List<Expense>> GetExpensesAsync()
    {
        return database.Table<Expense>().ToListAsync();
    }

    public Task<int> SaveExpenseAsync(Expense item)
    {
        if (database.Table<Expense>().FirstOrDefaultAsync(x => x.Id == item.Id) != null)
        {
            return database.UpdateAsync(item);
        }
        else
        {
            return database.InsertAsync(item);
        }
    }

    public Task<int> DeleteExpenseAsync(Guid id)
    {
        return database.DeleteAsync<Expense>(id);
    }

}
