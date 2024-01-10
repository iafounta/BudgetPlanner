namespace BudgetPlanner.Presentation.ViewModel
{
    public partial class IncomeViewModel : ObservableObject{

        private readonly ISender _mediator;
        private bool isNewIncome;
        public ICommand PageAppearingCommand { get; }

        public IncomeViewModel(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            PageAppearingCommand = new Command(async () => await GetIncomeAsync());
        }

        [ObservableProperty]
        ObservableCollection<IncomeModel> incomeItems;

        [ObservableProperty]
        IncomeModel editableIncome;

        IncomeModel copyIncome;


        [RelayCommand]
        async Task GoToDetailPageToAddNewIncome()
        {
            isNewIncome = true;
            EditableIncome = new IncomeModel();
            await Shell.Current.GoToAsync(nameof(IncomeDetailPage));
        }

        [RelayCommand]
        async Task GoToDetailPageToEditIncome(IncomeModel income)
        {
            isNewIncome = false;
            copyIncome = income;
            EditableIncome = new IncomeModel
            {
                Id = income.Id,
                Name = income.Name,
                Amount = income.Amount,
                TimeInterval = income.TimeInterval
            };
            await Shell.Current.GoToAsync(nameof(IncomeDetailPage));

        }


        [RelayCommand]
        async Task SaveCurrentIncome()
        {
            if (isNewIncome)
            {
                var result = await _mediator.Send(new Application.UseCases.Income.AddIncome(EditableIncome.Name, EditableIncome.Amount, EditableIncome.TimeInterval));

                if (!result.IsSuccess)
                {
                    // Here should appear an error message when the entry could not be added
                }
                EditableIncome.Id = result.Value;
                IncomeItems.Add(EditableIncome);
            }
            else
            {

                copyIncome.Name = EditableIncome.Name;
                copyIncome.Amount = EditableIncome.Amount;
                copyIncome.TimeInterval = EditableIncome.TimeInterval;

                var result = await _mediator.Send(new Application.UseCases.Income.UpdateIncome(EditableIncome.Id, EditableIncome.Name, EditableIncome.Amount, EditableIncome.TimeInterval));

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
        async Task Delete(IncomeModel e)
        {

            if (IncomeItems.Contains(e))
            {
                IncomeItems.Remove(e);
            }
            var result = await _mediator.Send(new Application.UseCases.Income.DeleteIncome(e.Id));
            if (!result.IsSuccess)
            {
                // Here should appear an error message when the entry could not be delted
            }
        }

        async Task GetIncomeAsync()
        {
            if (IncomeItems is null)
            {
                IncomeItems = new();
            }

            if (IncomeItems.Count != 0)
            {
                return;
            }

            var result = await _mediator.Send(new Application.UseCases.Income.GetAllIncomes());
            if (result.IsSuccess)
            {
                if (result.Value!.Count == 0)
                {
                    return;
                }

                foreach (Domain.Entities.Income? income in result.Value)
                {
                    IncomeItems.Add(new IncomeModel() { Id = income.Id, Name = income.Name, Amount = income.Amount, TimeInterval = income.TimeInterval });
                }
            }
        }

    }
}
