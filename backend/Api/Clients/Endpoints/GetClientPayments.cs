namespace Api.Clients.Endpoints;

public class GetClientPayments : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{id}/payments", Handle)
        .WithSummary("Get a clients's pyaments")
        .WithRequestValidation<Request>()
        .WithEnsureEntityExists<Client, Request>(c => c.Id);

    public record Request(int Id, int? Page, int? PageSize) : IPagedRequest;
    public class RequestValidator : PagedRequestValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
    public record Response(int Id, int ClientId, string Username, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc, decimal Amount, double Value);

    public static async Task<PagedList<Response>> Handle([AsParameters] Request request, AppDbContext database, CancellationToken cancellationToken)
    {
        return await database.Payments
            .Where(c => c.ClientId == request.Id)
            .OrderByDescending(c => c.CreatedAtUtc)
            .Select(p => new Response
            (
                p.Id,
                p.ClientId,
                p.Client.UserName,
                p.CreatedAtUtc,
                p.UpdatedAtUtc,
                p.Amount,
                p.Rate.Value
            ))
            .ToPagedListAsync(request, cancellationToken);
    }
}
