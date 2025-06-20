namespace Api.Clients.Endpoints;

public class GetClientById : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{id}", Handle)
        .WithSummary("Gets a client by id")
        .WithRequestValidation<Request>();

    public record Request(int Id);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
    public record Response(
        int Id,
        string Name,
        string Email,
        decimal Balance,
        DateTime CreatedAtUtc
    );

    private static async Task<Results<Ok<Response>, NotFound>> Handle([AsParameters] Request request, AppDbContext database, CancellationToken cancellationToken)
    {
        var client = await database.Clients
            .Where(c => c.Id == request.Id)
            .Select(c => new Response
            (
                c.Id,
                c.Name,
                c.Email,
                c.Balance,
                c.CreatedAtUtc
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return client is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(client);
    }
}