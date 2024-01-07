namespace BudgetPlanner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddTransient<IMapper<Expense, ExpenseVm>, ExpenseMapperToVm>();
        services.AddTransient<IMapper<ExpenseVm, Expense>, ExpenseMapperToEntity>();
        return services;
    }
}