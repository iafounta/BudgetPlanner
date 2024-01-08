using BudgetPlanner.Presentation.Service;

namespace BudgetPlanner.Presentation.ViewModel;

public partial class ExpensesViewModel : ObservableObject
{
    private readonly ISender _mediator;
    private bool isNewExpense;
    public ICommand PageAppearingCommand { get; }

    public ExpensesViewModel(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        PageAppearingCommand = new Command(async () => await GetExpensesAsync());
    }

    [ObservableProperty]
    ObservableCollection<ExpensesModel> expensesItems;

    [ObservableProperty]
    ExpensesModel currentExpense;


    [RelayCommand]
    async Task GoToDetailPageToAddNewExpense()
    {
        isNewExpense = true;
        CurrentExpense = new ExpensesModel();
        await Shell.Current.GoToAsync(nameof(ExpensesDetailPage));
        
    }


    [RelayCommand]
    async Task GoToDetailPageToEditExpense(ExpensesModel expense)
    {
        isNewExpense = false;
        CurrentExpense = expense;
        await Shell.Current.GoToAsync(nameof(ExpensesDetailPage));

    }


    [RelayCommand]
    async Task SaveCurrentExpense()
    {
        if (isNewExpense)
        {
            ExpensesItems.Add(CurrentExpense);
            var result = await _mediator.Send(new AddExpense(currentExpense.Name, currentExpense.Amount, currentExpense.TimeInterval));
            if (result.IsSuccess)
            {
                // Expense successfully saved
            }
        }

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }


    [RelayCommand]
    async Task Delete(ExpensesModel e)
    {

        if (ExpensesItems.Contains(e))
        {
            ExpensesItems.Remove(e);
        }
        var result = await _mediator.Send(new DeleteExpense(e.Id));
    }

    async Task GetExpensesAsync()
    {
        if (ExpensesItems is null)
        {
            ExpensesItems = new();
        }

        if (ExpensesItems.Count != 0)
        {
            return;
        }

        var result = await _mediator.Send(new GetAllExpense());
        if (result.IsSuccess)
        {
            if (result.Value!.Count == 0)
            {
                return;
            }

            foreach (var expense in result.Value)
            {
                ExpensesItems.Add(new ExpensesModel() { Name = expense.Name, Amount = expense.Amount, TimeInterval = expense.TimeInterval });
            }
        }
    }

}
