namespace Api.Payments.Endpoints;

public class UpdatePayment : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithSummary("Updates a payment")
        .WithRequestValidation<Request>();

    public record Request(int Id, decimal? Amount, double? RateValue);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(p => p.Amount)
                .NotNull()
                .GreaterThan(0)
                .When(p => p.Amount != null);

            RuleFor(p => p.RateValue)
                .NotNull()
                .GreaterThan(0)
                .When(p => p.RateValue != null);
        }
    }

    private static async Task<Ok> Handle(Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var payment = await database.Payments.SingleAsync(c => c.Id == request.Id, cancellationToken);
        payment.Amount = request.Amount ?? payment.Amount;
        payment.Rate.Value = request.RateValue ?? payment.Rate.Value;
        await database.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok();
    }
}