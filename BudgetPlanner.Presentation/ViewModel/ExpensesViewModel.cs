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
            expensesItems.Add(currentExpense);
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
        var result = await _mediator.Send(new GetAllExpense());
        if (result.IsSuccess)
        {
            ExpensesItems = new ObservableCollection<ExpensesModel>
            {
                new ExpensesModel() { Name = "Miete", Amount = 1500.00f,  TimeInterval = "Monatlich"},
                new ExpensesModel() { Name = "Krankenkasse", Amount = 320.50f,  TimeInterval = "Monatlich"},
                new ExpensesModel() { Name = "Internet", Amount = 80.00f,  TimeInterval = "Monatlich"},
                new ExpensesModel() { Name = "Einkaufen", Amount = 100.00f,  TimeInterval = "Wochenlich"},
            };
        }
    }

}
