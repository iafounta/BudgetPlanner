namespace BudgetPlanner.Presentation.View;

public partial class IncomePage : ContentPage
{
	public IncomePage(IncomeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}