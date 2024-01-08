using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BudgetPlanner.Presentation.Model
{
    public partial class ExpensesModel : ObservableObject{

        public Guid Id { get; set; } = Guid.NewGuid();

        [ObservableProperty]
        string name;

        [ObservableProperty]
        float amount;

        [ObservableProperty]
        string timeInterval;


    }
}
