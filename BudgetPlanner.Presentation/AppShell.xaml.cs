using BudgetPlanner.Presentation.View;

namespace BudgetPlanner.Presentation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ExpensesDetailPage), typeof(ExpensesDetailPage));
        }
    }
}
