namespace Api.Payments.Endpoints;

public class DeletePayment : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapDelete("/{id}", Handle)
        .WithSummary("Deletes a payment")
        .WithRequestValidation<Request>();

    public record Request(int Id);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0);
        }
    }

    private static async Task<Results<Ok, NotFound>> Handle([AsParameters] Request request, AppDbContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var rowsDeleted = await database.Payments
            .Where(p => p.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted == 1
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }
}