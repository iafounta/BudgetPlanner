namespace BudgetPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        // Repositories
        services.AddTransient<IExpenseRepository, ExpenseRepository>();
        return services;
    }
}