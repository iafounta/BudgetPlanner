namespace BudgetPlanner.Infrastructure.Entities
{
    public class IncomeDBEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Amount { get; set; }
        public string TimeInterval { get; set; } = string.Empty;
    }
}
