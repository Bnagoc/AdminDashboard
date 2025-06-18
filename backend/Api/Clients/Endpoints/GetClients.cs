namespace Api.Clients.Endpoints;

public class GetClients : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithSummary("Gets all clients")
        .WithRequestValidation<Request>();

    public record Request(int? Page, int? PageSize) : IPagedRequest;
    public class RequestValidator : PagedRequestValidator<Request>;
    public record Response(
        int Id,
        string Username,
        DateTime CreateAtUtc
    );

    private static async Task<PagedList<Response>> Handle([AsParameters] Request request, AppDbContext database, CancellationToken cancellationToken)
    {
        return await database.Clients
            .Select(c => new Response
            (
                c.Id,
                c.UserName,
                c.CreatedAtUtc
            ))
            .ToPagedListAsync(request, cancellationToken);
    }
}
