using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Request
{
    public class RequestRequest : IRequest<long?>
    {
        public long? Id { get; set; }
        public long? OriginId { get; set; }
        public decimal? RequestAmount { get; set; }
        public string? Name { get; set; } 
        public string? Reason { get; set; }
        public string? RequestDate { get; set; }
        public string? Email { get; set; } 
        public long? PriorityId { get; set; }
        public int? State { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
