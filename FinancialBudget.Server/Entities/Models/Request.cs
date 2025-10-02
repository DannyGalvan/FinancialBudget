using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class Request : IEntity<long>
    {
        public long Id { get; set; }
        public long OriginId { get; set; }
        public decimal RequestAmount { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTimeOffset RequestDate { get; set; }
        public long RequestStatusId { get; set; }
        public DateTimeOffset ApprovedDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string RejectionReason { get; set; } = string.Empty;
        public string AuthorizedReason { get; set; } = string.Empty;
        public long PriorityId { get; set; } 
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public Origin? Origin { get; set; }
        public RequestStatus? RequestStatus { get; set; }
        public Priority? Priority { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; } = new List<BudgetItem>();
        public virtual ICollection<Traceability> Traceabilities { get; } = new List<Traceability>();
    }
}
