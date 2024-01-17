namespace BudgetPlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        // Repositories
        services.AddSingleton<IExpenseRepository, ExpenseRepository>();
        services.AddSingleton<IIncomeRepository, IncomeRepository>();
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}