namespace FinancialBudget.Server.Entities.Response
{
    public class CatalogueResponse
    {
        public dynamic Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }
    }
}
