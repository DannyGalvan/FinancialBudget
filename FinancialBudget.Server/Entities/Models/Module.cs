using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class Module : IEntity<long>
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int State { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual ICollection<Operation> Operations { get; } = new List<Operation>();
    }
}
