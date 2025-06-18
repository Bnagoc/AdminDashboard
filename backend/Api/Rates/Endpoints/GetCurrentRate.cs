namespace Api.Rates.Endpoints;

public class GetCurrentRate : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Gets current rate");

    public record Response(
        int Id,
        double Value,
        DateTime CreatedAtUtc
    );

    private static async Task<Results<Ok<Response>, NotFound>> Handle(AppDbContext database, CancellationToken cancellationToken)
    {
        var rate = await database.Rates
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new Response
            (
                r.Id,
                r.Value,
                r.CreatedAtUtc
            ))
            .LastOrDefaultAsync(cancellationToken);

        return rate is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(rate);
    }
}