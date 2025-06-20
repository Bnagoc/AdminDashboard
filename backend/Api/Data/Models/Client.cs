namespace Api.Data.Models;

public class Client : IEntity
{
    public int Id { get; private init; }
    public Guid ReferenceId { get; private init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required decimal Balance { get; set; }
    public DateTime CreatedAtUtc { get; private init; } = DateTime.UtcNow;
    public List<Payment> Payments { get; init; } = [];
}
