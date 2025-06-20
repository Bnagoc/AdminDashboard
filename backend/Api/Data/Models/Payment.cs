namespace Api.Data.Models;

public class Payment : IEntity, IOwnedEntity
{
    public int Id { get; private set; }
    public Guid ReferenceId { get; private init; } = Guid.NewGuid();
    public required int ClientId { get; init; }
    public Client Client { get; init; } = null!;
    public required decimal Amount { get; set; }
    public DateTime CreatedAtUtc { get; private init; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public required int RateId { get; init; }
    public Rate Rate { get; init; } = null!;
}
