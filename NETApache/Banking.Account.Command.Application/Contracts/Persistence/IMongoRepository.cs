using Banking.Account.Command.Domain.Common;

namespace Banking.Account.Command.Application.Contracts.Persistence
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> GetAllAsync();
        Task<TDocument> GetById(string id);
        Task InsertDocument(TDocument document);
        Task DeleteById(string id);
        public Task UpdateDocument(TDocument document);
    }
}
