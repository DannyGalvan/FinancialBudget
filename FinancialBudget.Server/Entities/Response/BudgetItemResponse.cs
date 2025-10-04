namespace FinancialBudget.Server.Entities.Response
{
    public class BudgetItemResponse
    {
        public long Id { get; set; }
        public long BudgetId { get; set; }
        public decimal Amount { get; set; }
        public long OriginId { get; set; }
        public long SplitTypeId { get; set; }
        public long RequestId { get; set; }
        public int State { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
