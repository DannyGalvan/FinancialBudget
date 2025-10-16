namespace FinancialBudget.Server.Entities.Response
{
    public class BudgetResponse
    {
        public long Id { get; set; }
        public decimal AuthorizedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public decimal CommittedAmount { get; set; }
        public int Period { get; set; }
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }
    }
}
