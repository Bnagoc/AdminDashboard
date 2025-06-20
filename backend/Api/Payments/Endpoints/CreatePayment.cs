namespace Api.Payments.Endpoints;

public class CreatePayment : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/", Handle)
        .WithSummary("Creates a new payment")
        .WithRequestValidation<Request>();

    public record Request(int ClientId, decimal Amount, int RateId);
    public record Response(int Id);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(p => p.Amount)
                .NotNull()
                .GreaterThan(0);
        }
    }

    private static async Task<Results<Ok<Response>, BadRequest<string>>> Handle(Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var client = await database.Clients.FindAsync(request.ClientId);

        if (client == null)
        {
            return TypedResults.BadRequest("Client not found");
        }

        if (client.Balance < request.Amount)
        {
            return TypedResults.BadRequest("Insufficient balance");
        }

        var payment = new Payment
        {
            ClientId = request.ClientId,
            Amount = request.Amount,
            RateId = request.RateId
        };

        client.Balance -= payment.Amount;

        await database.Payments.AddAsync(payment, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);
        var response = new Response(payment.Id);
        return TypedResults.Ok(response);
    }
}