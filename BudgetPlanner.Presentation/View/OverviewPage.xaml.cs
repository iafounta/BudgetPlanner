using Microcharts;
using SkiaSharp;

namespace BudgetPlanner.Presentation.View;

public partial class OverviewPage : ContentPage
{
	public OverviewPage(OverviewViewModel vm){
		InitializeComponent();
        BindingContext = vm;
    }
}