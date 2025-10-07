using FinancialBudget.Server.Entities.Interfaces;

namespace FinancialBudget.Server.Entities.Request
{
    public class CatalogueRequest : IRequest<string?>, ICatalogue
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
