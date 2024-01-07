namespace BudgetPlanner.Presentation.ViewModel;

public partial class ExpensesViewModel : ObservableObject
{
    private readonly ISender _mediator;
    private bool isNeuExpense;
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
        isNeuExpense = true;
        currentExpense = new ExpensesModel();
        await Shell.Current.GoToAsync(nameof(ExpensesDetailPage));
        
    }


    [RelayCommand]
    async Task GoToDetailPageToEditExpense(ExpensesModel expense)
    {
        isNeuExpense = false;
        currentExpense = expense;
        await Shell.Current.GoToAsync(nameof(ExpensesDetailPage));

    }


    [RelayCommand]
    async Task SaveCurrentExpense()
    {
        if (isNeuExpense)
        {
            ExpensesItems.Add(currentExpense);
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
            foreach (var expense in result.Value)
            {
                ExpensesItems.Add(new ExpensesModel() { Name = expense.Name, Amount = expense.Amount, TimeInterval = expense.TimeInterval });
            }
        }
    }

}
