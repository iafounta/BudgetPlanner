namespace BudgetPlanner.Infrastructure.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly string databasePath;
    private SQLiteAsyncConnection? database;
    private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

    public IncomeRepository(IDatabasePathProvider pathProvider)
    {
        databasePath = pathProvider.GetDatabasePath();
    }

    async Task Init()
    {
        if (database is not null)
            return;

       database = new SQLiteAsyncConnection(databasePath, Flags);
       await database.CreateTableAsync<IncomeDBEntity>();
    }

    public async Task<Income> GetOneIncomeAsync(int id)
    {
        await Init();
        IncomeDBEntity incomeDBEntity = await database!.Table<IncomeDBEntity>().FirstOrDefaultAsync(x => x.Id == id);
        Income income = new()
        {
            Id = incomeDBEntity.Id,
            Amount = incomeDBEntity.Amount,
            Name = incomeDBEntity.Name,
            TimeInterval = incomeDBEntity.TimeInterval
        };
        return income;
    }

    public async Task<List<Income>> GetIncomesAsync()
    {
        await Init();
        List<IncomeDBEntity> IncomeDbEntityList = await database!.Table<IncomeDBEntity>().ToListAsync();
        List<Income> list = [];
        foreach (IncomeDBEntity incomeDBEntity in IncomeDbEntityList)
        {
            list.Add(new Income() {
                Id = incomeDBEntity.Id,
                Amount = incomeDBEntity.Amount,
                Name = incomeDBEntity.Name,
                TimeInterval = incomeDBEntity.TimeInterval
            });
        }
        return list;
    }

    public async Task<int> SaveIncomeAsync(Income item)
    {
        await Init();
        IncomeDBEntity incomeDBEntity = new()
        {
            Id = item.Id,
            Amount = item.Amount,
            Name = item.Name,
            TimeInterval = item.TimeInterval
        };

        bool isItemExisting = false;

        if (item.Id != 0)
        {
            isItemExisting = await database!.Table<IncomeDBEntity>().FirstOrDefaultAsync(x => x.Id == incomeDBEntity.Id) != null;
        }

        if (isItemExisting)
        {
            await database!.UpdateAsync(incomeDBEntity);
            return incomeDBEntity.Id;
        }
        else
        {
            await database!.InsertAsync(incomeDBEntity);
            return incomeDBEntity.Id;
        }

    }

    public async Task<int> DeleteIncomeAsync(int id)
    {
        await  Init();
        return await database!.DeleteAsync<IncomeDBEntity>(id);
    }
}
