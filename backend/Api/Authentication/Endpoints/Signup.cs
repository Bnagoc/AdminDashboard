using Api.Authentication.Services;

namespace Api.Authentication.Endpoints;

public class Signup : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/signup", Handle)
        .WithSummary("Creates a new user account")
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

    private static async Task<Results<Ok<Response>, ValidationError>> Handle(Request request, AppDbContext database, Jwt jwt,
        IRefreshTokenService refreshTokenService, CancellationToken cancellationToken)
    {
        var isUsernameTaken = await database.Users
            .AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (isUsernameTaken)
        {
            return new ValidationError("Username is already taken");
        }

        var user = new User
        {
            Email = request.Email,
            Password = request.Password,
        };
        await database.Users.AddAsync(user, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        var token = jwt.GenerateToken(user);
        var refreshToken = await refreshTokenService.GenerateAsync(user.Id.ToString());

        var response = new Response(token, refreshToken);
        return TypedResults.Ok(response);
    }
}