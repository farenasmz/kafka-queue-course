namespace Banking.Cqrs.Core.Event
{
    public class FundsDepositedEvent : BaseEvent
    {
        public decimal Amount { get; set; }

        public FundsDepositedEvent(string id, decimal amount) : base(id)
        {
            Amount = amount;
        }
    }
}
