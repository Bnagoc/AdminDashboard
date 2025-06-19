using Api.Authentication.Services;
using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Authentication.Endpoints;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/login", Handle)
        .WithSummary("Logs in a user")
        .WithRequestValidation<Request>();

    public record Request(string Email, string Password);
    public record Response(string Token, string RefreshToken);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle(Request request, AppDbContext database, Jwt jwt,
        IRefreshTokenService refreshTokenService, CancellationToken cancellationToken)
    {
        var user = await database.Users.SingleOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password, cancellationToken);

        if (user is null || user.Password != request.Password)
        {
            return TypedResults.Unauthorized();
        }

        var token = jwt.GenerateToken(user);
        database.RefreshTokens.RemoveRange(database.RefreshTokens.Where(u => u.UserId == user.Id.ToString()).ToList());
        var refreshToken = await refreshTokenService.GenerateAsync(user.Id.ToString());
        var response = new Response(token, refreshToken);
        return TypedResults.Ok(response);
    }
}
