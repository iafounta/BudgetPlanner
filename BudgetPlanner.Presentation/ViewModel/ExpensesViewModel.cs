namespace BudgetPlanner.Presentation.ViewModel;

public partial class ExpensesViewModel : ObservableObject
{
    private readonly ISender _mediator;
    private bool isNewExpense;
    public ICommand PageAppearingCommand { get; }

    [ObservableProperty]
    ObservableCollection<string> timeIntervalItems;
    private readonly Dictionary<string, string> timeIntervalMapping;

    public ExpensesViewModel(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        PageAppearingCommand = new Command(async () => await GetExpensesAsync());

        timeIntervalMapping = new Dictionary<string, string>{
                { TimeIntervalEnum.Daily.ToString(), "Täglich" },
                { TimeIntervalEnum.Weekly.ToString(), "Wöchentlich" },
                { TimeIntervalEnum.Monthly.ToString(), "Monatlich" },
                { TimeIntervalEnum.Yearly.ToString(), "Jährlich" }
             };

        timeIntervalItems = new ObservableCollection<string>([.. timeIntervalMapping.Values]);
    }

    [ObservableProperty]
    ObservableCollection<ExpensesModel> expensesItems;

    [ObservableProperty]
    ExpensesModel editableExpense;

    ExpensesModel copyExpense;


    [RelayCommand]
    async Task GoToDetailPageToAddNewExpense()
    {
        isNewExpense = true;
        EditableExpense = new ExpensesModel();
        await Shell.Current.GoToAsync(nameof(ExpensesDetailPage));
    }

    [RelayCommand]
    async Task GoToDetailPageToEditExpense(ExpensesModel expense)
    {
        isNewExpense = false;
        copyExpense = expense;
        EditableExpense = new ExpensesModel
        {
            Id = expense.Id,
            Name = expense.Name,
            Amount = expense.Amount,
            TimeInterval = expense.TimeInterval
        };
        await Shell.Current.GoToAsync(nameof(ExpensesDetailPage));

    }


    [RelayCommand]
    async Task SaveCurrentExpense()
    {
        if (isNewExpense)
        {
            var result = await _mediator.Send(new AddExpense(EditableExpense.Name, EditableExpense.Amount, EditableExpense.TimeInterval));

            if (!result.IsSuccess)
            {
                // Here should appear an error message when the entry could not be added
            }
            EditableExpense.Id = result.Value;
            ExpensesItems.Add(EditableExpense);
        }
        else
        {

            copyExpense.Name = EditableExpense.Name;
            copyExpense.Amount = EditableExpense.Amount;
            copyExpense.TimeInterval = EditableExpense.TimeInterval;

            var result = await _mediator.Send(new UpdateExpense(EditableExpense.Id, EditableExpense.Name, EditableExpense.Amount, EditableExpense.TimeInterval));

            if (!result.IsSuccess)
            {
                // Here should appear an error message when the entry could not be updated
            }
        }

        await NavigateBackAsync();
    }

    [RelayCommand]
    async Task Cancel()
    {
        await NavigateBackAsync();
    }

    private static async Task NavigateBackAsync()
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
        if (!result.IsSuccess)
        {
            // Here should appear an error message when the entry could not be delted
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
            if (result.Value!.Count == 0)
            {
                return;
            }

            foreach (Domain.Entities.Expense? expense in result.Value)
            {
                ExpensesItems.Add(new ExpensesModel() { Id = expense.Id, Name = expense.Name, Amount = expense.Amount, TimeInterval = expense.TimeInterval });
            }
        }
    }

}
