# Observer Pattern

## Descrizione

L'Observer Pattern definisce una dipendenza uno-a-molti tra oggetti in modo che quando un oggetto cambia stato, tutti i suoi dipendenti vengono notificati e aggiornati automaticamente. Questo pattern è anche conosciuto come Publisher-Subscriber.

## File Coinvolti

### Middleware Observer

- `src/Businesses/MyDiet.Core.Business/Middlewares/CoreUserMiddleware.cs`

### Service Provider Observer

- `src/Apis/MyDiet.Shared.Api/ExtensionMethods/ServiceProviderExtension.cs`

### Initialization Observer

- `src/Apis/MyDiet.Shared.Api/Program.cs`

### Authentication Observer

- `src/Businesses/MyDiet.Auth.Business/AuthenticationSchemes/CustomJwtAuthenticationHandler.cs`

## Implementazione

### Middleware Observer Pattern

```csharp
public class CoreUserMiddleware
{
    private readonly RequestDelegate _next;

    public CoreUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Observer che reagisce a ogni richiesta HTTP
    public async Task InvokeAsync(
        HttpContext context,
        IService<CoreUserDto, CoreUser, Guid> coreUserService,
        IService<AuthUserDto, AuthUser, Guid> authUserService,
        IMapper<UserClaims, CoreUserDto> userMapper)
    {
        // Osserva i claims dell'utente autenticato
        var user = context.User;
        var userIdClaim = user.FindFirst("userId");
        var usernameClaim = user.FindFirst("username");
        var roleClaim = user.FindFirst(ClaimTypes.Role);

        if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            // Reagisce al cambio di contesto utente
            var existingCoreUser = await coreUserService.GetByIdAsync(userId);

            if (existingCoreUser.Data is null)
            {
                // Notifica: crea nuovo CoreUser se non esiste
                var existingAuthUser = await authUserService.GetByIdAsync(userId);
                if (existingAuthUser.Data is not null)
                {
                    var userClaims = new UserClaims
                    {
                        UserId = userId,
                        Username = usernameClaim?.Value,
                        Role = Enum.Parse<UserRole>(roleClaim?.Value ?? "User")
                    };

                    var coreUserDto = userMapper.Map(userClaims);
                    await coreUserService.CreateAsync(coreUserDto);
                }
            }
        }

        // Continua la pipeline di osservatori
        await _next(context);
    }
}
```

### Application Initialization Observer

```csharp
// Program.cs - Subject che notifica l'avvio dell'applicazione
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStartupServices(builder.Configuration);

var app = builder.Build();

// Observer che reagisce all'inizializzazione
await app.Services.InitializeAsync();
```

### Service Provider Extension Observer

```csharp
public static class ServiceProviderExtension
{
    // Observer che reagisce all'inizializzazione del service provider
    public static async Task InitializeAsync(this IServiceProvider serviceProvider)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            // Osserva l'evento di inizializzazione e reagisce
            var keyPairManager = scope.ServiceProvider.GetRequiredService<IKeyPairManager>();
            await keyPairManager.RegenerateAsync();
        }
        catch (Exception ex)
        {
            // Observer per gestione errori di inizializzazione
            Console.WriteLine(ex.Message);
        }
    }
}
```

### Authentication Handler Observer

```csharp
public class CustomJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Observer che reagisce alle richieste di autenticazione
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Osserva l'header Authorization
        var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            // Reagisce all'assenza del token
            return AuthenticateResult.Fail("Missing or invalid authorization header.");
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        try
        {
            // Osserva e valida il token
            var publicKeysResponse = await _publicKeyService.GetAsync();
            var validationKeys = _keyPairMapper.Map(publicKeysResponse.Data);

            // Reagisce alla validazione del token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _tokenOption.Issuer,
                ValidateAudience = true,
                ValidAudience = _tokenOption.Audience,
                ValidateLifetime = true,
                IssuerSigningKeys = validationKeys,
                ValidateIssuerSigningKey = true
            };

            // Observer per il risultato della validazione
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, "CustomJwt"));
        }
        catch (Exception ex)
        {
            // Observer per errori di autenticazione
            return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
        }
    }
}
```

### Business Response Observer Pattern

```csharp
// Implicit Observer Pattern nel BusinessResponse<T>
public class BusinessResponse<T>
{
    // Il pattern è implementato implicitamente attraverso il tipo di risposta
    // I consumer "osservano" il tipo di risposta e reagiscono di conseguenza

    public static BusinessResponse<T> Ok(T data, string message)
    public static BusinessResponse<T> BadRequest(string message)
    public static BusinessResponse<T> NotFound(string message)
    public static BusinessResponse<T> Unauthorize(string message)
    public static BusinessResponse<T> InternalServerError(string message)
}

// Usage - Manager che osserva le risposte dei servizi
public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
{
    var userRes = await _userService.GetByIdAsync(userId);

    // Observer pattern: reagisce al tipo di risposta
    if (userRes.Data is null)
    {
        return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
    }

    var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name);

    // Observer pattern: reagisce alla presenza di dati
    if (existingDietRes.Data.ToList().Count != 0)
    {
        return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.DietAlreadyExists);
    }
}
```

## Implementazione Implicita tramite Events

Il pattern Observer è implementato implicitamente attraverso:

1. **HTTP Pipeline**: Ogni middleware osserva e reagisce alle richieste
2. **Dependency Injection**: I servizi osservano i cambiamenti di configurazione
3. **Authentication Events**: L'handler osserva gli eventi di autenticazione
4. **Application Lifecycle**: Gli extension methods osservano eventi di startup

## Vantaggi dell'Implementazione Attuale

1. **Reattività**: Il sistema reagisce automaticamente ai cambiamenti
2. **Disaccoppiamento**: Gli observer non conoscono i dettagli del subject
3. **Estensibilità**: Facile aggiungere nuovi observer
4. **Composizione**: Multiple catene di observer (middleware pipeline)

## Scenari di Utilizzo

1. **HTTP Middleware**: Observer per ogni richiesta HTTP
2. **Authentication**: Observer per eventi di autenticazione
3. **User Synchronization**: Observer per sincronizzazione utenti
4. **Application Initialization**: Observer per eventi di startup

## Vantaggi del Pattern

- **Disaccoppiamento**: Subject e observer non sono strettamente accoppiati
- **Estensibilità**: Facile aggiungere/rimuovere observer
- **Riusabilità**: Observer possono essere riutilizzati per diversi subject
- **Responsività**: Aggiornamenti automatici ai cambiamenti

## Applicazione nel Contesto ASP.NET Core

Il framework ASP.NET Core implementa estensivamente l'Observer Pattern attraverso:

- **Middleware Pipeline**: Ogni middleware è un observer della richiesta
- **Event Handlers**: Handler per eventi di applicazione
- **Service Lifecycle**: Observer per eventi di creazione/distruzione servizi
- **Authentication Schemes**: Observer per eventi di autenticazione

## Pattern Correlati

- Spesso utilizzato insieme al **Mediator Pattern** per gestire comunicazioni complesse
- Si integra con **Command Pattern** per notificare l'esecuzione di comandi
