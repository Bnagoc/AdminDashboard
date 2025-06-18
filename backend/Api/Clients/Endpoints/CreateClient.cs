namespace Api.Clients.Endpoints;

public class CreateClient : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/", Handle)
        .WithSummary("Creates a new client")
        .WithRequestValidation<Request>();

    public record Request(string UserName);
    public record Response(int Id);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(c => c.UserName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }

    private static async Task<Ok<Response>> Handle(Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var client = new Client
        {
            UserName = request.UserName
        };

        await database.Clients.AddAsync(client, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);
        var response = new Response(client.Id);
        return TypedResults.Ok(response);
    }
}