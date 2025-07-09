# Adapter Pattern

## Descrizione

L'Adapter Pattern consente a interfacce incompatibili di lavorare insieme. Agisce come un ponte tra due interfacce incompatibili, convertendo l'interfaccia di una classe in un'altra interfaccia che i client si aspettano.

## File Coinvolti

### Authentication Handler Adapter

- `src/Businesses/MyDiet.Auth.Business/AuthenticationSchemes/CustomJwtAuthenticationHandler.cs`

### Middleware Adapter

- `src/Businesses/MyDiet.Core.Business/Middlewares/CoreUserMiddleware.cs`

### Database Context Adapters

- `src/Infrastructures/MyDiet.Auth.Infrastructure/Models/MyDietAuthDb.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/Models/MyDietCoreDb.cs`

### Configuration Extension Adapters

- `src/Apis/MyDiet.Shared.Api/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Apis/MyDiet.Shared.Api/ExtensionMethods/ServiceProviderExtension.cs`

## Implementazione

### Authentication Handler Adapter

```csharp
public class CustomJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;
    private readonly IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> _keyPairMapper;
    private readonly TokenOption _tokenOption;

    public CustomJwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IVaultService<JsonWebKeySetDto> publicKeyService,
        IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>> keyPairMapper,
        TokenOption tokenOption)
        : base(options, logger, encoder, clock)
    {
        // Adatta i servizi custom al framework di autenticazione ASP.NET Core
        _publicKeyService = publicKeyService;
        _keyPairMapper = keyPairMapper;
        _tokenOption = tokenOption;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Adatta la logica di autenticazione custom
        // all'interfaccia richiesta dal framework
    }
}
```

### Database Context Adapter

```csharp
internal class MyDietAuthDb : IDatabase<MyDietAuthDbContext>
{
    private readonly MyDietAuthDbContext _context;

    public MyDietAuthDb(MyDietAuthDbContext context)
    {
        _context = context;
    }

    // Adatta DbContext all'interfaccia IDatabase
    public MyDietAuthDbContext Context { get => _context; }
}
```

### Middleware Adapter

```csharp
public class CoreUserMiddleware
{
    private readonly RequestDelegate _next;

    public CoreUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Adatta i servizi di business al pipeline di ASP.NET Core
    public async Task InvokeAsync(
        HttpContext context,
        IService<CoreUserDto, CoreUser, Guid> coreUserService,
        IService<AuthUserDto, AuthUser, Guid> authUserService,
        IMapper<UserClaims, CoreUserDto> userMapper)
    {
        // Adatta i claims HTTP ai servizi di business
        var user = context.User;
        var userIdClaim = user.FindFirst("userId");
        var usernameClaim = user.FindFirst("username");
        var roleClaim = user.FindFirst(ClaimTypes.Role);

        // Trasforma i dati del contesto HTTP per i servizi business
        if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            // Logica di adattamento tra HTTP context e business services
        }

        await _next(context);
    }
}
```

### Service Collection Extension Adapter

```csharp
public static class ServiceCollectionExtension
{
    // Adatta la configurazione custom al sistema DI di ASP.NET Core
    private static IServiceCollection AddTokenValidation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Adatta la configurazione Swagger per JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Example: \"Bearer yourtoken\"",
            });
        });

        // Adatta l'authentication handler custom al framework
        services
            .AddAuthentication("CustomJwt")
            .AddScheme<AuthenticationSchemeOptions, CustomJwtAuthenticationHandler>("CustomJwt", null);

        return services;
    }
}
```

### Service Provider Extension Adapter

```csharp
public static class ServiceProviderExtension
{
    // Adatta l'inizializzazione custom al lifecycle di ASP.NET Core
    public static async Task InitializeAsync(this IServiceProvider serviceProvider)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var keyPairManager = scope.ServiceProvider.GetRequiredService<IKeyPairManager>();
            await keyPairManager.RegenerateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
```

## Vantaggi dell'Implementazione Attuale

1. **Integrazione framework**: Adatta componenti custom ai framework esistenti
2. **Riutilizzo**: Permette di riutilizzare codice esistente con nuove interfacce
3. **Disaccoppiamento**: Isola le implementazioni custom dai dettagli del framework
4. **Estensibilità**: Facilita l'aggiunta di nuove funzionalità

## Scenari di Utilizzo nella Soluzione

1. **Authentication**: Adatta la logica JWT custom al sistema di autenticazione ASP.NET
2. **Database Access**: Adatta DbContext all'interfaccia repository custom
3. **Middleware**: Adatta servizi business al pipeline HTTP
4. **Configuration**: Adatta configurazioni custom al sistema DI

## Vantaggi del Pattern

- **Compatibilità**: Fa funzionare insieme interfacce incompatibili
- **Riutilizzo**: Permette di riutilizzare codice esistente
- **Separazione**: Mantiene separato il codice client dall'implementazione
- **Flessibilità**: Facilita l'integrazione di componenti esterni

## Applicazione Corretta

Il pattern è implementato correttamente:

- Chiara separazione tra interfacce incompatibili
- Adattatori che traducono tra diverse interfacce
- Integrazione pulita con i framework esistenti
- Mantenimento della funzionalità originale

## Pattern Correlati

- Spesso utilizzato insieme al **Facade Pattern** per semplificare interfacce complesse
- Può essere combinato con **Decorator Pattern** per aggiungere funzionalità
