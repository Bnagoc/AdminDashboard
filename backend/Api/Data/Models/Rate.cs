namespace Api.Data.Models;

public class Rate : IEntity
{
    public int Id { get; private set; }
    public Guid ReferenceId { get; private init; } = Guid.NewGuid();
    public required double Value { get; set; }

    public DateTime CreatedAtUtc { get; private init; } = DateTime.UtcNow;
    public List<Payment> Payments { get; init; } = [];
}