using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Infrastructure.Entities
{
    public class ExpenseDBEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Amount { get; set; }
        public string TimeInterval { get; set; } = string.Empty;
    }
}
