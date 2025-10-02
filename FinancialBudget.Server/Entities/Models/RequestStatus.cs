using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class RequestStatus : IEntity<long>, ICatalogue
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual ICollection<Request> Requests { get; } = new List<Request>();
        public virtual ICollection<Traceability> Traceabilities { get; } = new List<Traceability>();
    }
}
