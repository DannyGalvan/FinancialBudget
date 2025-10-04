public class BudgetItemRequest
    {
        public long Id { get; set; }
        public long BudgetId { get; set; }
        public decimal Amount { get; set; }
        public long OriginId { get; set; }
        public long SplitTypeId { get; set; }
        public long RequestId { get; set; }
        public int State { get; set; } = 1;
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }