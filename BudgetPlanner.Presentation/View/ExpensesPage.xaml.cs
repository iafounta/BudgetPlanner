namespace BudgetPlanner.Presentation.View;

public partial class ExpensesPage : ContentPage
{
	public ExpensesPage(ExpensesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}