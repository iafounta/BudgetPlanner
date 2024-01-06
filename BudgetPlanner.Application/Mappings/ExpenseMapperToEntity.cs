namespace BudgetPlanner.Application.Mappings;

public class ExpenseMapperToEntity : IMapper<ExpenseVm, Expense>
{
    public Expense Map(ExpenseVm source)
    {
        return new Expense
        {
            Id = source.Id,
            Name = source.Name,
            Amount = source.Amount,
            TimeInterval = source.TimeInterval
        };
    }

    public IEnumerable<Expense> Map(IEnumerable<ExpenseVm> source)
    {
        return source.Select(Map);
    }
}
