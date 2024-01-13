using BudgetPlanner.Application.UseCases.Income;
using BudgetPlanner.Presentation.Helpers;
using Microcharts;
using SkiaSharp;

namespace BudgetPlanner.Presentation.ViewModel
{
    public partial class OverviewViewModel : ObservableObject {
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

            HandlePeriodChangeAsync(CALC_MONTH);
        }

        [RelayCommand]
        async Task InitializeExpensesAndIncomesAsync()
        {
            expensesItems = await GetExpensesAsync();
            incomeItems = await GetIncomesAsync();
            HandlePeriodChangeAsync(CALC_MONTH);
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


        private void UpdateOverviewChartPerMonth()
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

        private void UpdateOverviewChartPerYear(){
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
            };
        }


        private void UpdateExpsensesChartPerMonth()
        {
            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var expense in expensesItems.OrderByDescending(x => x.Amount))
            {
                float monthAmount = CalculateAmountPerMonth(expense.TimeInterval, expense.Amount);
                chartEntries.Add(new ChartEntry(monthAmount)
                {
                    Label = expense.Name,
                    ValueLabel = expense.Amount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                }) ;
            }

            ExpsensesChart = new BarChart()
            {
                Entries = chartEntries
            };
        }

        private void UpdateExpsensesChartPerYear()
        {
            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var expense in expensesItems.OrderByDescending(x => x.Amount))
            {
                float monthAmount = CalculateAmountPerYear(expense.TimeInterval, expense.Amount);
                chartEntries.Add(new ChartEntry(monthAmount)
                {
                    Label = expense.Name,
                    ValueLabel = expense.Amount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                });
            }

            ExpsensesChart = new BarChart()
            {
                Entries = chartEntries
            };
        }


        private void UpdateIncomeChartPerMonth()
        {
            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var income in incomeItems.OrderByDescending(x => x.Amount))
            {
                float monthAmount = CalculateAmountPerMonth(income.TimeInterval, income.Amount);
                chartEntries.Add(new ChartEntry(monthAmount)
                {
                    Label = income.Name,
                    ValueLabel = income.Amount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                });
            }

            IncomeChart = new BarChart()
            {
                Entries = chartEntries
            };
        }

        private void UpdateIncomeChartPerYear()
        {
            IList<ChartEntry> chartEntries = new List<ChartEntry>();
            foreach (var income in incomeItems.OrderByDescending(x => x.Amount))
            {
                float monthAmount = CalculateAmountPerYear(income.TimeInterval, income.Amount);
                chartEntries.Add(new ChartEntry(monthAmount)
                {
                    Label = income.Name,
                    ValueLabel = income.Amount.ToString(),
                    Color = ColorUtilsHelper.GetRandomColor()
                });
            }

            IncomeChart = new BarChart()
            {
                Entries = chartEntries
            };
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
}
