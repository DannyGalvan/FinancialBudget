namespace FinancialBudget.Server.Entities.Response
{
    public class RequestResponse
    {
        public long Id { get; set; }
        public long OriginId { get; set; }
        public decimal RequestAmount { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string RequestDate { get; set; } = string.Empty;
        public long RequestStatusId { get; set; }
        public string ApprovedDate { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RejectionReason { get; set; } = string.Empty;
        public string AuthorizedReason { get; set; } = string.Empty;
        public long PriorityId { get; set; }
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }

        public CatalogueResponse? Origin { get; set; }
        public CatalogueResponse? RequestStatus { get; set; }
        public CatalogueResponse? Priority { get; set; }
    }
}
