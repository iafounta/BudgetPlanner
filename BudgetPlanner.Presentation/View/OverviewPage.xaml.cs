using Microcharts;
using SkiaSharp;

namespace BudgetPlanner.Presentation.View;

public partial class OverviewPage : ContentPage
{
	public OverviewPage(OverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        //chartView.Chart = new HalfRadialGaugeChart
        //{
        //    Entries = entries
        //};

    }

    ChartEntry[] entries = new[]
       {
            new ChartEntry(212)
            {
                Label = "Einahmen",
                ValueLabel = "112",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(248)
            {
                Label = "Ausgaben",
                ValueLabel = "648",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(128)
            {
                Label = "Übrick",
                ValueLabel = "428",
                Color = SKColor.Parse("#b455b6")
            },

        };
}