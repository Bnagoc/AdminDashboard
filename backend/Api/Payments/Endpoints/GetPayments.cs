namespace Api.Payments.Endpoints;

public class GetPayments : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Gets all/last N payments")
        .WithRequestValidation<Request>();

    public record Request(int? Page, int? Take) : IPagedRequest;
    public class RequestValidator : PagedRequestValidator<Request>;
    public record Response(
        int Id,
        decimal Amount,
        string Client,
        DateTime CreatedAtUtc,
        double Rate
    );

    private static async Task<PagedList<Response>> Handle([AsParameters] Request request, AppDbContext database, CancellationToken cancellationToken)
    {
        return await database.Payments
            .OrderByDescending(p => p.CreatedAtUtc)
            .Select(p => new Response
            (
                p.Id,
                p.Amount,
                p.Client.Name,
                p.CreatedAtUtc,
                p.Rate.Value
            ))
            .ToPagedListAsync(request, cancellationToken);
    }
}
