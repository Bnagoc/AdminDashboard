using Api.Authentication.Endpoints;
using Api.Clients.Endpoints;
using Api.Common.Filters;
using Api.Payments.Endpoints;
using Api.Rates.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Api;
public static class Endpoints
{
    private static readonly OpenApiSecurityScheme securityScheme = new()
    {
        Type = SecuritySchemeType.Http,
        Name = JwtBearerDefaults.AuthenticationScheme,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new()
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("")
            .AddEndpointFilter<RequestLoggingFilter>()
            .WithOpenApi();

        endpoints.MapAuthenticationEndpoints();
        endpoints.MapClientEndpoints();
        endpoints.MapPaymentEndpoints();
        endpoints.MapRateEndpoints();
    }

    private static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/auth")
            .WithTags("Authentication");
            
        endpoints.MapPublicGroup()
            .MapEndpoint<Signup>()
            .MapEndpoint<Login>()
            .MapEndpoint<GenerateRefreshToken>();
    }

    private static void MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/clients")
            .WithTags("Clients");

        endpoints.MapPublicGroup()
            .MapEndpoint<GetClients>()
            .MapEndpoint<GetClientById>()
            .MapEndpoint<GetClientPayments>();

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<CreateClient>()
            .MapEndpoint<UpdateClient>()
            .MapEndpoint<DeleteClient>();
    }

    private static void MapRateEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/rates")
            .WithTags("Rates");

        endpoints.MapPublicGroup()
            .MapEndpoint<GetCurrentRate>();

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<UpdateRate>();
    }

    private static void MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/payments")
            .WithTags("Payments");

        endpoints.MapPublicGroup()
            .MapEndpoint<GetPayments>()
            .MapEndpoint<GetPaymentById>();

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<CreatePayment>()
            .MapEndpoint<UpdatePayment>()
            .MapEndpoint<DeletePayment>();
    }

    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .RequireAuthorization()
            .WithOpenApi(x => new(x)
            {
                Security = [new() { [securityScheme] = [] }],
            });
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}