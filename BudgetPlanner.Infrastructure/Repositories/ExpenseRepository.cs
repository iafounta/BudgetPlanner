namespace BudgetPlanner.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly string databasePath;
    private readonly SQLiteAsyncConnection database;
    private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

    public ExpenseRepository(IDatabasePathProvider pathProvider)
    {
        databasePath = pathProvider.GetDatabasePath();
        database = new SQLiteAsyncConnection(databasePath, Flags);
        database.CreateTableAsync<Expense>().Wait();
    }

    public async Task<Expense> GetOneExpenseAsync(Guid id)
    {
        return await database.Table<Expense>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Expense>> GetExpensesAsync()
    {
        return database.Table<Expense>().ToListAsync();
    }

    public async Task<int> SaveExpenseAsync(Expense item)
    {
        var existingItem = await database.Table<Expense>().FirstOrDefaultAsync(x => x.Id == item.Id);

        if (existingItem != null)
        {
            return await database.UpdateAsync(item);
        }
        else
        {
            return await database.InsertAsync(item);
        }

    }

    public Task<int> DeleteExpenseAsync(Guid id)
    {
        return database.DeleteAsync<Expense>(id);
    }
}
