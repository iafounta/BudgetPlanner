using BudgetPlanner.Infrastructure.Interfaces;

namespace BudgetPlanner.Presentation.Service
{
    public class DatabasePathProvider : IDatabasePathProvider
    {
        public string GetDatabasePath()
        {
            return Path.Combine(FileSystem.AppDataDirectory, "BudgetPlannerSQLite.db3");
        }
    }
}
