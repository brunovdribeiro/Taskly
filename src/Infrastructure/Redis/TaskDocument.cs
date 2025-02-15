using Redis.OM;
using Redis.OM.Modeling;

namespace Infrastructure.Redis;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "Task" })]
public class TaskDocument
{
    [RedisIdField]
    public string Id { get; init; }
    [Indexed]
    public string Title { get; init; }
    [Indexed]
    public string Description { get; init; }
    [Indexed]
    public string Status { get; init; }
    [Indexed]
    public string Priority { get; init; }
    [Indexed(CascadeDepth = 1)]
    public DateTime CreatedAt { get; init; }
    [Indexed(CascadeDepth = 1)]
    public DateTime? LastModified { get; init; }
    [Indexed]
    public string? AssignedTo { get; init; }
}