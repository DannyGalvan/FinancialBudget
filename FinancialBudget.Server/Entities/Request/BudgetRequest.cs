using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Request
{
    public class BudgetRequest : IRequest<long?>
    {
        public long? Id { get; set; }
        public decimal? AuthorizedAmount { get; set; } 
        public decimal? AvailableAmount { get; set; } 
        public int? Period { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
