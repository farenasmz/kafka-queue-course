namespace Banking.Cqrs.Core.Messages
{
    public class Message
    {
        public string Id { get; set; }

        public Message(string id)
        {
            Id = id;
        }
    }
}
