namespace Api.Rates.Endpoints;

public class UpdateRate : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/", Handle)
        .WithSummary("Update current rate by creating a new one")
        .WithRequestValidation<Request>();

    public record Request(double Value);
    public record Response(int Id);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(p => p.Value)
                .NotNull()
                .GreaterThan(0);
        }
    }

    private static async Task<Ok<Response>> Handle(Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var rate = new Rate
        {
            Value = request.Value
        };

        await database.Rates.AddAsync(rate, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);
        var response = new Response(rate.Id);
        return TypedResults.Ok(response);
    }
}