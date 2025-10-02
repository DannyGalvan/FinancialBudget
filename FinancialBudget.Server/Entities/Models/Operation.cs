using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Models
{
    public class Operation : IEntity<long>
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long ModuleId { get; set; }
        public string Policy { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsVisible { get; set; }
        public int State { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }


        public virtual Module? Module { get; set; }
        public virtual ICollection<RolOperation> RolOperations { get; set; } = new List<RolOperation>();
    }
}
