using AutoMapper;
using BudgetPlanner.Infrastructure.Entities;
using Microsoft.VisualBasic;

namespace BudgetPlanner.Infrastructure.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly string databasePath;
    private  SQLiteAsyncConnection database;
    private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
    private readonly IMapper mapper;

    public IncomeRepository(IDatabasePathProvider pathProvider, IMapper mapper)
    {
        databasePath = pathProvider.GetDatabasePath();
        //database = new SQLiteAsyncConnection(databasePath, Flags);
        //database.CreateTableAsync<IncomeDBEntity>().Wait();
        this.mapper = mapper;
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
        IncomeDBEntity a = await database.Table<IncomeDBEntity>().FirstOrDefaultAsync(x => x.Id == id);
        return mapper.Map<Income>(a);
    }

    public async Task<List<Income>> GetIncomesAsync()
    {
        await Init();
        List<IncomeDBEntity> IncomeDbEntityList = await database.Table<IncomeDBEntity>().ToListAsync();
        var Incomes = IncomeDbEntityList.Select(item => mapper.Map<Income>(item)).ToList();
        return Incomes;
    }

    public async Task<int> SaveIncomeAsync(Income item)
    {
        await Init();
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

    public async Task<int> DeleteIncomeAsync(int id)
    {
        await  Init();
        return await database.DeleteAsync<IncomeDBEntity>(id);
    }
}
