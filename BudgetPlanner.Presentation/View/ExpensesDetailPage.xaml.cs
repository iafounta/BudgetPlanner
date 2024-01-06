using BudgetPlanner.Presentation.ViewModel;

namespace BudgetPlanner.Presentation.View;

public partial class ExpensesDetailPage : ContentPage
{
	public ExpensesDetailPage(ExpensesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}