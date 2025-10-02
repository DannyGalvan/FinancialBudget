using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class Traceability : IEntity<long>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RequestId { get; set; }
        public long RequestStatusId { get; set; }
        public string Comments { get; set; } = string.Empty;
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }


        public User? User { get; set; }
        public Request? Request { get; set; }
        public RequestStatus? RequestStatus { get; set; }
    }
}
