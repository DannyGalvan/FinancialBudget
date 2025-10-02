using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class Budget : IEntity<long>
    {
        public long Id { get; set; }
        public decimal AuthorizedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public decimal CommittedAmount { get; set; }
        public int Period { get; set; }
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();
    }
}
