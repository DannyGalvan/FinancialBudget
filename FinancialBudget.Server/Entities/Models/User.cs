using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class User : IEntity<long>
    {

        public long Id { get; set; }
        public long RolId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int State { get; set; } = 1;
        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset? UpdatedAt { get; set; } 
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual ICollection<Traceability>? Traceabilities { get; set; }
    }
}