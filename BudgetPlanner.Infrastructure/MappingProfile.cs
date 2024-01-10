using AutoMapper;
using BudgetPlanner.Infrastructure.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ExpenseDBEntity, Expense>();
        CreateMap<Expense, ExpenseDBEntity> ();

        CreateMap<Income, IncomeDBEntity>();
        CreateMap<IncomeDBEntity, Income>();
    }
}