namespace Banking.Cqrs.Core.Event
{
    public class AccountOpenedEvent : BaseEvent
    {
        public string AccountHandler { get; set; }
        public string AccountType { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal OpeningBalance { get; set; }

        public AccountOpenedEvent(string id, string accountHandler, string accountType, DateTime createdDate, decimal openingBalance) : base(id)
        {
            AccountType = accountType;
            CreatedDate = createdDate;
            OpeningBalance = openingBalance;
            AccountHandler = accountHandler;
        }
    }
}
