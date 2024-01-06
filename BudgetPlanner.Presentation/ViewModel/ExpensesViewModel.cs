using BudgetPlanner.Presentation.Model;
using BudgetPlanner.Presentation.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BudgetPlanner.Presentation.ViewModel
{
    public partial class ExpensesViewModel : ObservableObject
    {
        private readonly ISender _mediator;
        private bool isNeuExpense;

        public ExpensesViewModel(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            var result = _mediator.Send(new GetAllExpense()).GetAwaiter().GetResult();
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

    }
}
