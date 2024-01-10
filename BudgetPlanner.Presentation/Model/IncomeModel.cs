using SQLite;

namespace BudgetPlanner.Presentation.Model
{
    public partial class IncomeModel : ObservableObject {
        public int Id { get; set; } 

        [ObservableProperty]
        string name;

        [ObservableProperty]
        float amount;

        [ObservableProperty]
        string timeInterval;
    }
}
