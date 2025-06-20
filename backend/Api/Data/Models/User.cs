namespace Api.Data.Models;

public class User : IEntity
{
    public int Id { get; private init; }
    public Guid ReferenceId { get; private init; } = Guid.NewGuid();
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAtUtc { get; private init; } = DateTime.UtcNow;
}
