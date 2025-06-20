namespace Api.Clients.Endpoints;

public class UpdateClient : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithSummary("Updates a client")
        .WithRequestValidation<Request>();

    public record Request(int Id, string? Name, string? Email);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100)
                .When(c => c.Name != null);

            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(100)
                .When(c => c.Email != null);
        }
    }

    private static async Task<Ok> Handle(Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var client = await database.Clients.SingleAsync(c => c.Id == request.Id, cancellationToken);
        client.Name = request.Name ?? client.Name;
        client.Email = request.Email ?? client.Email;
        await database.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}