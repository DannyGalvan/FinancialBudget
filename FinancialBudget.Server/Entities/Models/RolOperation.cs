using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class RolOperation : IEntity<long>
    {
        public long Id { get; set; }
        public long RolId { get; set; }
        public long OperationId { get; set; }
        public int State { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }


        public virtual Rol? Rol { get; set; }
        public virtual Operation? Operation { get; set; }
    }
}
