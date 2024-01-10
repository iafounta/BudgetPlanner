namespace BudgetPlanner.Presentation.Model;

public partial class ExpensesModel : ObservableObject{
    public int Id { get; set; } 

    [ObservableProperty]
    string name;

    [ObservableProperty]
    float amount;

    [ObservableProperty]
    string timeInterval;


}
