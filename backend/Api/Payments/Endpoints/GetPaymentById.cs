namespace Api.Payments.Endpoints;

public class GetPaymentById : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{id}", Handle)
        .WithSummary("Gets a payment by id")
        .WithRequestValidation<Request>();

    public record Request(int Id);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0);
        }
    }
    public record Response(
        int Id,
        decimal Amount,
        string Client,
        DateTime CreatedAtUtc,
        double Rate
    );

    private static async Task<Results<Ok<Response>, NotFound>> Handle([AsParameters] Request request, AppDbContext database, CancellationToken cancellationToken)
    {
        var payment = await database.Payments
            .Where(p => p.Id == request.Id)
            .Select(p => new Response
            (
                p.Id,
                p.Amount,
                p.Client.UserName,
                p.CreatedAtUtc,
                p.Rate.Value
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return payment is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(payment);
    }
}