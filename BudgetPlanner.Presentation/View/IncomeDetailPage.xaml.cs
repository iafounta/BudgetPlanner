namespace BudgetPlanner.Presentation.View;

public partial class IncomeDetailPage : ContentPage
{
	public IncomeDetailPage(IncomeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}