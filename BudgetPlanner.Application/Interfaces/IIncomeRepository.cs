namespace BudgetPlanner.Application.Interfaces
{
    public interface IIncomeRepository
    {
        public Task<Income> GetOneIncomeAsync(int id);
        public Task<List<Income>> GetIncomesAsync();
        public Task<int> SaveIncomeAsync(Income item);
        public Task<int> DeleteIncomeAsync(int id);
    }
}
