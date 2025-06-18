namespace Api.Clients.Endpoints;

public class UpdateClient : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithSummary("Updates a client")
        .WithRequestValidation<Request>();

    public record Request(int Id, string UserName);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
            RuleFor(c => c.UserName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }

    private static async Task<Ok> Handle(Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var client = await database.Clients.SingleAsync(c => c.Id == request.Id, cancellationToken);
        client.UserName = request.UserName;
        await database.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}