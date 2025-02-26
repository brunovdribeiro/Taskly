using Infrastructure.Persistence.Redis.Documents;

namespace Infrastructure.Persistence.Redis.Interfaces;

public interface IUserDocumentRepository
{
    Task<UserDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<UserDocument>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(UserDocument document, CancellationToken cancellationToken);
    Task UpdateAsync(UserDocument document, CancellationToken cancellationToken);
}