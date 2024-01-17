namespace BudgetPlanner.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly string databasePath;
    private SQLiteAsyncConnection? database;
    private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

    public ExpenseRepository(IDatabasePathProvider pathProvider)
    {
        databasePath = pathProvider.GetDatabasePath();
    }

    async Task Init()
    {
        if (database is not null)
            return;

        database = new SQLiteAsyncConnection(databasePath, Flags);
        await database.CreateTableAsync<ExpenseDBEntity>();
    }

    public async Task<Expense> GetOneExpenseAsync(int id) {
        await Init();
        ExpenseDBEntity expenseDBEntity = await database!.Table<ExpenseDBEntity>().FirstOrDefaultAsync(x => x.Id == id);
        Expense expense = new()
        {
            Id = expenseDBEntity.Id,
            Amount = expenseDBEntity.Amount,
            Name = expenseDBEntity.Name,
            TimeInterval = expenseDBEntity.TimeInterval
        };
        return expense;
    }

    public async Task<List<Expense>> GetExpensesAsync()
    {
        await Init();
        List<ExpenseDBEntity> expenseDbEntityList = await database!.Table<ExpenseDBEntity>().ToListAsync();
        List<Expense> list = [];
        foreach (ExpenseDBEntity expenseDBEntity in expenseDbEntityList)
        {
            list.Add(new Expense() {
                Id = expenseDBEntity.Id,
                Amount = expenseDBEntity.Amount,
                Name = expenseDBEntity.Name,
                TimeInterval = expenseDBEntity.TimeInterval
            });
        }
        return list;
    }

    public async Task<int> SaveExpenseAsync(Expense item)
    {
        await Init();
        ExpenseDBEntity expenseDBEntity = new()
        {
            Id = item.Id,
            Amount = item.Amount,
            Name = item.Name,
            TimeInterval = item.TimeInterval
        };

        bool isItemExisting = false;

        if (item.Id !=  0) 
        {
            isItemExisting = await database!.Table<ExpenseDBEntity>().FirstOrDefaultAsync(x => x.Id == expenseDBEntity.Id) != null ;
        }

        if (isItemExisting)
        {
            await database!.UpdateAsync(expenseDBEntity);
            return expenseDBEntity.Id;
        }
        else
        {
            await database!.InsertAsync(expenseDBEntity);
            return expenseDBEntity.Id;
        }

    }

    public async Task<int> DeleteExpenseAsync(int id)
    {
        await Init();
        return await database!.DeleteAsync<ExpenseDBEntity>(id);
    }
}
