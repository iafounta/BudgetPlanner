using AutoMapper;
using BudgetPlanner.Infrastructure.Entities;

namespace BudgetPlanner.Infrastructure.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly string databasePath;
    private readonly SQLiteAsyncConnection database;
    private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
    private readonly IMapper mapper;

    public IncomeRepository(IDatabasePathProvider pathProvider, IMapper mapper)
    {
        databasePath = pathProvider.GetDatabasePath();
        database = new SQLiteAsyncConnection(databasePath, Flags);
        database.CreateTableAsync<IncomeDBEntity>().Wait();
        this.mapper = mapper;
    }

    public async Task<Income> GetOneIncomeAsync(int id)
    {
        IncomeDBEntity a = await database.Table<IncomeDBEntity>().FirstOrDefaultAsync(x => x.Id == id);
        return mapper.Map<Income>(a);
    }

    public async Task<List<Income>> GetIncomesAsync()
    {
        List<IncomeDBEntity> IncomeDbEntityList = await database.Table<IncomeDBEntity>().ToListAsync();
        var Incomes = IncomeDbEntityList.Select(item => mapper.Map<Income>(item)).ToList();
        return Incomes;
    }

    public async Task<int> SaveIncomeAsync(Income item)
    {
        IncomeDBEntity IncomeDbEntity = mapper.Map<IncomeDBEntity>(item);
        bool isItemExisting = false;

        if (item.Id != 0)
        {
            isItemExisting = await database.Table<IncomeDBEntity>().FirstOrDefaultAsync(x => x.Id == IncomeDbEntity.Id) != null;
        }

        if (isItemExisting)
        {
            await database.UpdateAsync(IncomeDbEntity);
            return IncomeDbEntity.Id;
        }
        else
        {
            await database.InsertAsync(IncomeDbEntity);
            return IncomeDbEntity.Id;
        }

    }

    public Task<int> DeleteIncomeAsync(int id)
    {
        return database.DeleteAsync<IncomeDBEntity>(id);
    }
}
