# Dependency Injection Pattern

## Descrizione

Il Dependency Injection Pattern è una tecnica per implementare l'Inversion of Control (IoC). Invece di creare direttamente le dipendenze, un oggetto riceve le sue dipendenze dall'esterno. Questo pattern promuove il disaccoppiamento e migliora la testabilità del codice.

## File Coinvolti

### Configurazione Principale

- `src/Apis/MyDiet.Shared.Api/Program.cs`
- `src/Apis/MyDiet.Shared.Api/ExtensionMethods/ServiceCollectionExtension.cs`

### Extension Methods per DI - Auth Domain

- `src/Businesses/MyDiet.Auth.Business/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Infrastructures/MyDiet.Auth.Infrastructure/ExtensionMethods/ServiceCollectionExtension.cs`

### Extension Methods per DI - Core Domain

- `src/Businesses/MyDiet.Core.Business/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/ExtensionMethods/ServiceCollectionExtension.cs`

### Utilizzo nelle Classi

- `src/Businesses/MyDiet.Core.Business/Managers/DietManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/PlanManager.cs`
- `src/Businesses/MyDiet.Auth.Business/Managers/AuthManager.cs`
- `src/Businesses/MyDiet.Core.Business/Middlewares/CoreUserMiddleware.cs`

## Implementazione

### Configurazione Principale - Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStartupServices(builder.Configuration);

var app = builder.Build();
await app.Services.InitializeAsync();
```

### Extension Method Principale

```csharp
public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthStartupServices(configuration);
    services.AddCoreStartupServices(configuration);
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    return services;
}
```

### Auth Business Registration

```csharp
public static IServiceCollection AddAuthBusiness(this IServiceCollection services)
{
    // Mappers
    services.AddScoped<IMapper<AuthUserDto, AuthUser>, AuthUserMapper>();
    services.AddScoped<IMapper<AuthUser, AuthUserDto>, AuthUserMapper>();

    // Services
    services.AddScoped<IService<AuthUserDto, AuthUser, Guid>, AuthUserService>();
    services.AddScoped<ITokenService, TokenService>();

    // Managers
    services.AddScoped<IAuthManager, AuthManager>();
    services.AddScoped<ITokenManager, TokenManager>();

    return services;
}
```

### Core Business Registration

```csharp
public static IServiceCollection AddCoreBusiness(this IServiceCollection services)
{
    // Mappers
    services.AddScoped<IMapper<Diet, DietDto>, DietMapper>();
    services.AddScoped<IMapper<DietDto, Diet>, DietMapper>();

    // Services
    services.AddScoped<IService<DietDto, Diet, int>, DietService>();
    services.AddScoped<IService<PlanDto, Plan, int>, PlanService>();

    // Managers
    services.AddScoped<IManager<DietDto, CreateDietRequest, int>, DietManager>();
    services.AddScoped<IManager<PlanDto, CreatePlanRequest, int>, PlanManager>();

    return services;
}
```

### Utilizzo nel Constructor - DietManager

```csharp
public DietManager(
    IService<CoreUserDto, CoreUser, Guid> userService,
    IService<DietDto, Diet, int> dietService,
    IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper,
    IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper,
    DietMessageOption responseMessageOption)
{
    _userService = userService;
    _dietService = dietService;
    _createDtoToDietDtoMapper = createDtoToDietDtoMapper;
    _createRequestToCreateDtoMapper = createRequestToCreateDtoMapper;
    _responseMessageOption = responseMessageOption;
}
```

### Utilizzo nel Middleware

```csharp
public async Task InvokeAsync(
    HttpContext context,
    IService<CoreUserDto, CoreUser, Guid> coreUserService,
    IService<AuthUserDto, AuthUser, Guid> authUserService,
    IMapper<UserClaims, CoreUserDto> userMapper)
```

## Vantaggi dell'Implementazione Attuale

1. **Organizzazione modulare**: Ogni livello registra le proprie dipendenze
2. **Lifetimes appropriati**: Utilizzo di Scoped, Singleton e Transient appropriati
3. **Separazione dei domini**: Auth e Core hanno registrazioni separate
4. **Extension Methods**: Organizzazione pulita delle registrazioni
5. **Configuration binding**: Integrazione con IConfiguration per le opzioni

## Lifetimes Utilizzati

- **Scoped**: Managers, Services, Repositories, Mappers
- **Singleton**: MessageOptions, Configurations, Key Options
- **Transient**: Non utilizzato esplicitamente (default per alcuni servizi)

## Vantaggi del Pattern

- **Testabilità**: Facile mock delle dipendenze nei test
- **Flessibilità**: Cambio di implementazioni senza modificare il codice
- **Disaccoppiamento**: Le classi non dipendono da implementazioni concrete
- **Configurabilità**: Gestione centralizzata delle dipendenze

## Applicazione Corretta

Il pattern è implementato correttamente seguendo le best practices:

- Registrazione organizzata tramite extension methods
- Lifetimes appropriati per ogni tipo di servizio
- Constructor injection utilizzato consistentemente
- Separazione logica tra domini
- Configurazione centralizzata nel Program.cs
