using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Banking.Account.Command.Domain.Common
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get;}
    }
}
