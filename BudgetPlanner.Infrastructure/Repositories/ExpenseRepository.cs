using AutoMapper;
using BudgetPlanner.Infrastructure.Entities;

namespace BudgetPlanner.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly string databasePath;
    private  SQLiteAsyncConnection database;
    private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
    private readonly IMapper mapper;

    public ExpenseRepository(IDatabasePathProvider pathProvider, IMapper mapper)
    {
        databasePath = pathProvider.GetDatabasePath();
        //database = new SQLiteAsyncConnection(databasePath, Flags);
        //database.CreateTableAsync<ExpenseDBEntity>().Wait();
        this.mapper = mapper;
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
        ExpenseDBEntity a = await database.Table<ExpenseDBEntity>().FirstOrDefaultAsync(x => x.Id == id);
        return mapper.Map<Expense>(a);
    }

    public async Task<List<Expense>> GetExpensesAsync()
    {
        await Init();
        List<ExpenseDBEntity> expenseDbEntityList = await database.Table<ExpenseDBEntity>().ToListAsync();
        var expenses = expenseDbEntityList.Select(item => mapper.Map<Expense>(item)).ToList();
        return expenses;
    }

    public async Task<int> SaveExpenseAsync(Expense item)
    {
        await Init();
        ExpenseDBEntity expenseDbEntity = mapper.Map<ExpenseDBEntity>(item);
        bool isItemExisting = false;

        if (item.Id !=  0) 
        {
            isItemExisting = await database.Table<ExpenseDBEntity>().FirstOrDefaultAsync(x => x.Id == expenseDbEntity.Id) != null ;
        }

        if (isItemExisting)
        {
            await database.UpdateAsync(expenseDbEntity);
            return expenseDbEntity.Id;
        }
        else
        {
            await database.InsertAsync(expenseDbEntity);
            return expenseDbEntity.Id;
        }

    }

    public async Task<int> DeleteExpenseAsync(int id)
    {
        await Init();
        return await database.DeleteAsync<ExpenseDBEntity>(id);
    }
}
