namespace BudgetPlanner.Application.Mappings;

public class ExpenseMapperToVm : IMapper<Expense, ExpenseVm>
{
    public ExpenseVm Map(Expense source)
    {
        return new ExpenseVm
        {
            Id = source.Id,
            Name = source.Name,
            Amount = source.Amount,
            TimeInterval = source.TimeInterval
        };
    }

    public IEnumerable<ExpenseVm> Map(IEnumerable<Expense> source)
    {
        return source.Select(Map);
    }
}
