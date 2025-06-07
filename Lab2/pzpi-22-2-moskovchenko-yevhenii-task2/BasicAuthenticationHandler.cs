using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IServiceScopeFactory scopeFactory) // Впроваджуємо IServiceScopeFactory
        : base(options, logger, encoder, clock)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            // Використовуємо IServiceScopeFactory для отримання UserService
            // Це потрібно, тому що AuthenticationHandler - це синглтон, а UserService - scoped
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var user = await userService.Authenticate(username, password);

            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            // Створюємо ClaimsIdentity з реальними даними користувача з БД
            var claims = new[] {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }
    }
}