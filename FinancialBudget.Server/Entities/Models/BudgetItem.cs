using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class BudgetItem : IEntity<long>
    {
        public long Id { get; set; }
        public long BudgetId { get; set; }
        public decimal Amount { get; set; }
        public long OriginId { get; set; }
        public long SplitTypeId { get; set; }
        public long RequestId { get; set; }
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public Budget? Budget { get; set; }
        public Origin? Origin { get; set; }
        public SplitType? SplitType { get; set; }
        public Request? Request { get; set; }
    }
}
