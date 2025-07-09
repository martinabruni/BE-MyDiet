# Facade Pattern

## Descrizione

Il Facade Pattern fornisce un'interfaccia unificata e semplificata a un set di interfacce in un sottosistema. Definisce un'interfaccia di livello superiore che rende il sottosistema più facile da usare, nascondendo la complessità delle interazioni tra più componenti.

## File Coinvolti

### Manager Facades

- `src/Businesses/MyDiet.Auth.Business/Managers/AuthManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/DietManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/PlanManager.cs`

### Interface Definitions

- `src/Domains/MyDiet.Auth.Domain/Managers/IAuthManager.cs`
- `src/Domains/MyDiet.Core.Domain/Managers/IManager.cs`

### Configuration Facades

- `src/Apis/MyDiet.Shared.Api/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Businesses/MyDiet.Auth.Business/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Businesses/MyDiet.Core.Business/ExtensionMethods/ServiceCollectionExtension.cs`

## Implementazione

### AuthManager Facade

```csharp
internal class AuthManager : IAuthManager
{
    // Facade che nasconde la complessità di:
    // - User service
    // - Multiple mappers
    // - Token manager
    // - Message options

    private readonly IService<AuthUserDto, AuthUser, Guid> _authUserService;
    private readonly IMapper<UserRegistrationRequest, AuthUserDto> _registrationRequestMapper;
    private readonly IMapper<AuthUserDto, UserRegistrationResponse> _userResponseMapper;
    private readonly IMapper<AuthUserDto, UserClaims> _userClaimsMapper;
    private readonly ITokenManager _tokenManager;
    private readonly AuthManagerMessageOption _responseMessageOptions;

    public async Task<BusinessResponse<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest request)
    {
        // Facade method che coordina:
        // 1. Validazione input
        // 2. Verifica utente esistente
        // 3. Mapping request -> DTO
        // 4. Salvataggio utente
        // 5. Mapping DTO -> response

        if (request is null)
        {
            return BusinessResponse<UserRegistrationResponse>.BadRequest(_responseMessageOptions.InvalidRequest);
        }

        var existingUser = await _authUserService.FindAsync(u => u.Email == request.Email);
        // Nasconde la complessità delle operazioni multiple

        var userDto = _registrationRequestMapper.Map(request);
        var createResult = await _authUserService.CreateAsync(userDto);
        var response = _userResponseMapper.Map(createResult.Data);

        return BusinessResponse<UserRegistrationResponse>.Ok(response, _responseMessageOptions.UserCreatedSuccessfully);
    }
}
```

### DietManager Facade

```csharp
internal class DietManager : BaseManager<CreateDietRequest>, IManager<DietDto, CreateDietRequest, int>
{
    // Facade che coordina:
    // - User validation
    // - Diet service operations
    // - Multiple mappers
    // - Business rules validation

    private readonly IService<DietDto, Diet, int> _dietService;
    private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
    private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
    private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;
    private readonly DietMessageOption _responseMessageOption;

    public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
    {
        // Facade method che nasconde:
        // 1. Validazione utente e request
        // 2. Verifica esistenza utente
        // 3. Controllo duplicati
        // 4. Multiple mapping operations
        // 5. Business rules enforcement

        var validationResult = ValidateAndGetUserId(request, userIdClaim);
        var userRes = await _userService.GetByIdAsync(userId);
        var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);

        // Nasconde la complessità del mapping chain
        var createDto = _createRequestToCreateDtoMapper.Map(request);
        createDto.UserId = userId;
        var dietDto = _createDtoToDietDtoMapper.Map(createDto);

        return await _dietService.CreateAsync(dietDto);
    }
}
```

### Configuration Facade

```csharp
public static class ServiceCollectionExtension
{
    // Facade per la configurazione completa dell'applicazione
    public static IServiceCollection AddStartupServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Nasconde la complessità di configurazione di:
        // - Auth services
        // - Core services
        // - Controllers
        // - Swagger

        services.AddAuthStartupServices(configuration);
        services.AddCoreStartupServices(configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    private static IServiceCollection AddAuthStartupServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Facade per configurazione auth completa
        services.AddKeyPairInfrastructure(configuration);
        services.AddKeyPairBusiness();
        services.AddAuthInfrastructure(configuration);
        services.AddAuthBusiness();
        services.AddTokenValidation();
        return services;
    }
}
```

### Auth Business Facade

```csharp
public static IServiceCollection AddAuthBusiness(this IServiceCollection services)
{
    // Facade che nasconde la registrazione di:
    // - Mappers multipli
    // - Services
    // - Managers
    // - Options

    services.AddScoped<IMapper<AuthUserDto, AuthUser>, AuthUserMapper>();
    services.AddScoped<IMapper<AuthUser, AuthUserDto>, AuthUserMapper>();
    services.AddScoped<IMapper<UserRegistrationRequest, AuthUserDto>, AuthUserMapper>();
    services.AddScoped<IMapper<AuthUserDto, UserRegistrationResponse>, AuthUserMapper>();
    services.AddScoped<IMapper<AuthUserDto, UserClaims>, AuthUserMapper>();
    services.AddScoped<IMapper<UserClaims, List<Claim>>, ClaimMapper>();
    services.AddScoped<IMapper<JwtSecurityToken, TokenResponse>, TokenMapper>();

    services.AddScoped<IService<AuthUserDto, AuthUser, Guid>, AuthUserService>();
    services.AddScoped<ITokenService, TokenService>();

    services.AddSingleton<AuthManagerMessageOption>();
    services.AddScoped<IAuthManager, AuthManager>();
    services.AddScoped<ITokenManager, TokenManager>();

    return services;
}
```

## Vantaggi dell'Implementazione Attuale

1. **Semplificazione**: I manager forniscono interfacce semplici per operazioni complesse
2. **Incapsulamento**: La complessità interna è nascosta ai client
3. **Coordinamento**: I manager coordinano multiple operazioni in workflow coerenti
4. **Configurazione unificata**: Extension methods forniscono facade per setup complessi
5. **Business logic centralizzata**: Regole di business centralizzate nei manager

## Complessità Nascoste dai Facade

1. **AuthManager**:

   - Validazione input
   - Verifica duplicati
   - Mapping multipli
   - Gestione token
   - Gestione errori

2. **DietManager/PlanManager**:

   - Validazione utenti
   - Controlli di autorizzazione
   - Verifica business rules
   - Mapping chain complessi
   - Coordinamento servizi

3. **Configuration Facades**:
   - Setup dependency injection
   - Configurazione middleware
   - Setup autenticazione
   - Configurazione database

## Vantaggi del Pattern

- **Semplicità**: Interfaccia semplice per sottosistemi complessi
- **Disaccoppiamento**: Client non dipendono da componenti interni
- **Manutenibilità**: Modifiche interne non impattano i client
- **Riusabilità**: Facade possono essere riutilizzati in contesti diversi

## Applicazione Corretta

Il pattern è implementato correttamente:

- Manager che forniscono interfacce semplificate
- Configurazione complessa nascosta dietro extension methods
- Workflow complessi coordinati in singoli metodi
- Chiara separazione tra interfaccia pubblica e implementazione interna

## Pattern Correlati

- Spesso utilizzato insieme al **Command Pattern** per incapsulare operazioni
- Si integra bene con **Template Method Pattern** per definire workflow standard
