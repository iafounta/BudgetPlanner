namespace BudgetPlanner.Presentation.ViewModel
{
    public partial class OverviewViewModel : ObservableObject {

        private const float WeeksPerMonth = 4.34812141f;
        private const float DaysPerMonth = 30.4368499f;
        private const float WeeksPerYear = 52.177457f;
        private const float DaysPerYear = 365f;

        private readonly ISender _mediator;

        private const string CALC_MONTH = "Monatlich";
        private const string CALC_YEAR = "Jährlich";
        private List<ExpensesModel> expensesItems;
        private List<IncomeModel> incomeItems;


        [ObservableProperty]
        string selectedPeriod;

        [ObservableProperty]
        ObservableCollection<string> periodItems;

        [ObservableProperty]
        Chart overviewChart;

        [ObservableProperty]
        Chart expsensesChart;

        [ObservableProperty]
        Chart incomeChart;

        public OverviewViewModel(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            periodItems = new ObservableCollection<string>(){
                CALC_MONTH,
                CALC_YEAR
            };
        }

        [RelayCommand]
        async Task InitializeExpensesAndIncomesAsync()
        {
            try
            {
                expensesItems = await GetExpensesAsync();
                incomeItems = await GetIncomesAsync();
                SelectedPeriod = CALC_MONTH;
                HandlePeriodChangeAsync(SelectedPeriod);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", $"{ex.Message}\n{ex.StackTrace}" , "OK");
            }
        }

        private async Task<List<ExpensesModel>> GetExpensesAsync() {
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
            if (!result.IsSuccess || result.Value is null) {
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

            HandlePeriodChangeAsync(value);
        }

        private void HandlePeriodChangeAsync(string selectedPeriod)
        {

            if (SelectedPeriod == CALC_MONTH)
            {
                UpdateOverviewChartPerMonth();
                UpdateExpsensesChartPerMonth();
                UpdateIncomeChartPerMonth();
            }
            else if (SelectedPeriod == CALC_YEAR)
            {
                UpdateOverviewChartPerYear();
                UpdateExpsensesChartPerYear();
                UpdateIncomeChartPerYear();
            }
        }


        private void UpdateOverviewChart(Func<string, float, float> calculateAmount)
        {
            float expensesAmount = expensesItems.Sum(expense => calculateAmount(expense.TimeInterval, expense.Amount));
            float incomesAmount = incomeItems.Sum(income => calculateAmount(income.TimeInterval, income.Amount));
            float saldo = incomesAmount - expensesAmount;

            OverviewChart = new PieChart()
            {
                Entries = new[]
                {
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
        },
                LabelTextSize = 35
            };
        }

        private void UpdateOverviewChartPerMonth()
        {
            if(expensesItems.Count == 0 && incomeItems.Count == 0) {
                OverviewChart = new PieChart();
                return; 
            }
            UpdateOverviewChart(CalculateAmountPerMonth);
        }

        private void UpdateOverviewChartPerYear()
        {
            if (expensesItems.Count == 0 && incomeItems.Count == 0)
            {
                OverviewChart = new PieChart();
                return;
            }

            UpdateOverviewChart(CalculateAmountPerYear);
        }

        private void UpdateExpsensesChartPerMonth()
        {
            if (expensesItems.Count == 0)
            {
                ExpsensesChart = new BarChart();
                return;
            }

            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var expense in expensesItems.OrderByDescending(x => x.Amount))
            {
                float monthAmount = CalculateAmountPerMonth(expense.TimeInterval, expense.Amount);
                chartEntries.Add(new ChartEntry(monthAmount)
                {
                    Label = expense.Name,
                    ValueLabel = monthAmount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                }) ;
            }

            ExpsensesChart = new BarChart()
            {
                Entries = chartEntries,
                LabelTextSize = 35
            };
        }

        private void UpdateExpsensesChartPerYear()
        {
            if (expensesItems.Count == 0)
            {
                ExpsensesChart = new BarChart();
                return;
            }
            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var expense in expensesItems.OrderByDescending(x => x.Amount))
            {
                float yearAmount = CalculateAmountPerYear(expense.TimeInterval, expense.Amount);
                chartEntries.Add(new ChartEntry(yearAmount)
                {
                    Label = expense.Name,
                    ValueLabel = yearAmount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                });
            }

            ExpsensesChart = new BarChart()
            {
                Entries = chartEntries,
                 LabelTextSize = 35
            };
        }


        private void UpdateIncomeChartPerMonth()
        {
            if(incomeItems.Count == 0) { 
                IncomeChart = new BarChart();
                return; 
            }

            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var income in incomeItems.OrderByDescending(x => x.Amount))
            {
                float monthAmount = CalculateAmountPerMonth(income.TimeInterval, income.Amount);
                chartEntries.Add(new ChartEntry(monthAmount)
                {
                    Label = income.Name,
                    ValueLabel = monthAmount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                });
            }

            IncomeChart = new BarChart()
            {
                Entries = chartEntries,
                LabelTextSize = 35
            };
        }

        private void UpdateIncomeChartPerYear()
        {
            if (incomeItems.Count == 0)
            {
                IncomeChart = new BarChart();
                return;
            }
            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var income in incomeItems.OrderByDescending(x => x.Amount))
            {
                float yearAmount = CalculateAmountPerYear(income.TimeInterval, income.Amount);
                chartEntries.Add(new ChartEntry(yearAmount)
                {
                    Label = income.Name,
                    ValueLabel = yearAmount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                });
            }

            IncomeChart = new BarChart()
            {
                Entries = chartEntries,
                LabelTextSize = 35
            };
        }


        private float CalculateAmountPerMonth(string timeInterval, float amount)
        {
            switch (timeInterval)
            {
                case "Jährlich":
                    return amount / 12;
                case "Monatlich":
                    return amount;
                case "Wöchentlich":
                    return amount * WeeksPerMonth;
                case "Täglich":
                    return amount * DaysPerMonth;
                default:
                    return amount;
            }
        }

        private float CalculateAmountPerYear(string timeInterval, float amount)
        {
            switch (timeInterval)
            {
                case "Jährlich":
                    return amount;
                case "Monatlich":
                    return amount * 12;
                case "Wöchentlich":
                    return amount * WeeksPerYear;
                case "Täglich":
                    return amount * DaysPerYear;
                default:
                    return amount;
            }
        }
    }
}
