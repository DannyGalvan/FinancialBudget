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
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public BudgetResponse? Budget { get; set; }
        public CatalogueResponse? Origin { get; set; }
        public CatalogueResponse? SplitType { get; set; }
        public RequestResponse? Request { get; set; }
    }
}
