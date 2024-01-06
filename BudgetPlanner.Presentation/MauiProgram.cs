using BudgetPlanner.Presentation.View;
using BudgetPlanner.Presentation.ViewModel;
using Microsoft.Extensions.Logging;

namespace BudgetPlanner.Presentation
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ExpensesViewModel>();
            builder.Services.AddSingleton<ExpensesPage>();
            builder.Services.AddTransient<ExpensesDetailPage>();

            return builder.Build();
        }
    }
}
