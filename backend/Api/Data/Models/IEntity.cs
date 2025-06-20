namespace Api.Data.Models;

public interface IEntity
{
    int Id { get; }
    Guid ReferenceId { get; }
}

public interface IOwnedEntity
{
    int ClientId { get; }
}