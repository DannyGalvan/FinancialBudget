using System.ComponentModel;

namespace FinancialBudget.Server.Entities.Request
{
    public class QueryParamsCatalogueRequest
    {
        [DefaultValue(null)]
        public string? Filters { get; set; }
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue(30)]
        public int PageSize { get; set; }
        [DefaultValue(false)]
        public bool IncludeTotal { get; set; }
    }
}
