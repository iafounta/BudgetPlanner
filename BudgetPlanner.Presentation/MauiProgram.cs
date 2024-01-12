using BudgetPlanner.Infrastructure.Interfaces;
using BudgetPlanner.Presentation.Service;
using Microcharts.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace BudgetPlanner.Presentation;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMicrocharts()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ExpensesViewModel>();
        builder.Services.AddSingleton<ExpensesPage>();
        builder.Services.AddTransient<ExpensesDetailPage>();

        builder.Services.AddSingleton<IncomeViewModel>();
        builder.Services.AddSingleton<IncomePage>();
        builder.Services.AddTransient<IncomeDetailPage>();

        builder.Services.AddSingleton<OverviewViewModel>();
        builder.Services.AddSingleton<OverviewPage>();

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddSingleton<IDatabasePathProvider, DatabasePathProvider>();

        return builder.Build();
    }
}
