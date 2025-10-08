namespace FinancialBudget.Server.Utils
{
    /// <summary>
    /// Defines the <see cref="OrderAttribute" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute(int priority) : Attribute
    {
        /// <summary>
        /// Gets the Priority
        /// </summary>
        public int Priority { get; } = priority;
    }
}