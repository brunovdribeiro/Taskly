using Redis.OM.Modeling;

namespace Infrastructure.Persistences.Redis.Documents;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "User" })]
public class UserDocument
{
    [RedisIdField] public string Id { get; init; }

    [Indexed] public string Email { get; init; }

    [Indexed] public string Name { get; init; }

    [Indexed] public bool IsActive { get; init; }

    [Indexed(CascadeDepth = 1)] public DateTime CreatedAt { get; init; }

    [Indexed(CascadeDepth = 1)] public DateTime? LastModified { get; init; }
}