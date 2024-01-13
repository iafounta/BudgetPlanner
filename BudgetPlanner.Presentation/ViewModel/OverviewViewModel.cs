namespace BudgetPlanner.Presentation.ViewModel;

public partial class OverviewViewModel : ObservableObject {
    private readonly ISender _mediator;

    private const string CALC_MONTH = "Nach Monat";
    private const string CALC_YEAR = "Nach Jahr";

    [ObservableProperty]
    string selectedPeriod;

    [ObservableProperty]
    ObservableCollection<string> periodItems;

    [ObservableProperty]
    Chart overviewChart;


    public OverviewViewModel(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        overviewChart = new BarChart();

        periodItems = new ObservableCollection<string>(){
            CALC_MONTH,
            CALC_YEAR
        };
    }

    private async Task<List<ExpensesModel>> GetExpensesAsync(){
        var result = await _mediator.Send(new GetAllExpense());
        if (!result.IsSuccess || result.Value is null)
        {
            return [];
        }

        return result.Value.Select(expense => new ExpensesModel
        {
            Id = expense.Id,
            Name = expense.Name,
            Amount = expense.Amount,
            TimeInterval = expense.TimeInterval
        }).ToList();
    }

    private async Task<List<IncomeModel>> GetIncomesAsync()
    {
        var result = await _mediator.Send(new GetAllIncomes());
        if (!result.IsSuccess || result.Value is null){
            return [];
        }

        return result.Value.Select(income => new IncomeModel
        {
            Id = income.Id,
            Name = income.Name,
            Amount = income.Amount,
            TimeInterval = income.TimeInterval
        }).ToList();
    }


    partial void OnSelectedPeriodChanged(string value)
    {
        _ = HandlePeriodChangeAsync(value);
    }

    private async Task HandlePeriodChangeAsync(string selectedPeriod)
    {
        var expensesItems =  await GetExpensesAsync();
        var incomeItems = await GetIncomesAsync();

        if (SelectedPeriod == CALC_MONTH)
        {
            float expensesAmount = 0;
            float incomesAmount = 0;
            float saldo = 0;

            foreach (var expense in expensesItems)
            {
                expensesAmount += CalculateAmountPerMonth(expense.TimeInterval, expense.Amount);
            }

            foreach (var income in incomeItems)
            {
                incomesAmount += CalculateAmountPerMonth(income.TimeInterval, income.Amount);
            }

            saldo = incomesAmount - expensesAmount;

            OverviewChart = new BarChart()
            {
                Entries = new[] {
                        new ChartEntry(incomesAmount)
                        {
                            Label = "Einahmen",
                            ValueLabel = incomesAmount.ToString(),
                            Color = SKColor.Parse("#2c3e50")
                        },
                        new ChartEntry(expensesAmount)
                        {
                            Label = "Ausgaben",
                            ValueLabel = expensesAmount.ToString(),
                            Color = SKColor.Parse("#77d065")
                        },
                        new ChartEntry(saldo)
                        {
                            Label = "Saldo",
                            ValueLabel = saldo.ToString(),
                            Color = SKColor.Parse("#b455b6")
                        },
                 }
            };
        }
        else if (SelectedPeriod == CALC_YEAR)
        {
            float expensesAmount = 0;
            float incomesAmount = 0;
            float saldo = 0;

            foreach (var expense in expensesItems)
            {
                expensesAmount += CalculateAmountPerYear(expense.TimeInterval, expense.Amount);
            }

            foreach (var income in incomeItems)
            {
                incomesAmount += CalculateAmountPerYear(income.TimeInterval, income.Amount);
            }

            saldo = incomesAmount - expensesAmount;

            OverviewChart = new DonutChart()
            {
                Entries = new[] {
                        new ChartEntry(incomesAmount)
                        {
                            Label = "Einahmen",
                            ValueLabel = incomesAmount.ToString(),
                            Color = SKColor.Parse("#2c3e50")
                        },
                        new ChartEntry(expensesAmount)
                        {
                            Label = "Ausgaben",
                            ValueLabel = expensesAmount.ToString(),
                            Color = SKColor.Parse("#77d065")
                        },
                        new ChartEntry(saldo)
                        {
                            Label = "Saldo",
                            ValueLabel = saldo.ToString(),
                            Color = SKColor.Parse("#b455b6")
                        },
                 }
            }; var a = expensesItems;
        }
    }


    private float CalculateAmountPerMonth(string timeInterval, float amount)
    {

        if (timeInterval == "Jährlich")
        {
            return amount / 12;
        }
        else if (timeInterval == "Monatlich")
        {
            return amount;
        }
        else if (timeInterval == "Wöchentlich")
        {
            return (float)(amount * 4.34812141);
        }
        else if (timeInterval == "Täglich")
        {
            return (float)(amount * 30.4368499);
        }
        return amount;
    }

    private float CalculateAmountPerYear(string timeInterval, float amount)
    {

        if (timeInterval == "Jährlich")
        {
            return amount;
        }
        else if (timeInterval == "Monatlich")
        {
            return amount * 12;
        }
        else if (timeInterval == "Wöchentlich")
        {
            return (float)(amount *  52.177457);
        }
        else if (timeInterval == "Täglich")
        {
            return amount * 365;
        }
        return amount;
    }

}
