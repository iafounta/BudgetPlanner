using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Presentation.Model
{
    public partial class ExpensesModel : ObservableObject{

        int Id;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        float amount;

        [ObservableProperty]
        string timeInterval;


    }
}
