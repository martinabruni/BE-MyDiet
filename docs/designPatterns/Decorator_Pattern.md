# Decorator Pattern

## Descrizione

Il Decorator Pattern consente di aggiungere dinamicamente nuove funzionalità a oggetti senza alterare la loro struttura. Questo pattern crea una classe decorator che wrappa la classe originale e fornisce funzionalità aggiuntive mantenendo l'integrità dei metodi della classe.

## File Coinvolti

### Service Decorators

- `src/Businesses/MyDiet.Auth.Business/Services/AuthUserService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/CoreUserService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/DietService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/PlanService.cs`

### Manager Decorators

- `src/Businesses/MyDiet.Core.Business/Managers/DietManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/PlanManager.cs`
- `src/Businesses/MyDiet.Auth.Business/Managers/AuthManager.cs`

### Base Classes (BaseUtility)

- `AGenericService<TDto, TEntity, TKey>` - Componente base per servizi
- `AGenericRepository<TContext, TEntity, TKey>` - Componente base per repository

### Authentication Decorator

- `src/Businesses/MyDiet.Auth.Business/AuthenticationSchemes/CustomJwtAuthenticationHandler.cs`

## Implementazione

### Service Decorator Pattern

```csharp
// AuthUserService decora AGenericService con funzionalità specifiche per AuthUser
internal class AuthUserService : AGenericService<AuthUserDto, AuthUser, Guid>
{
    // Decora il servizio base con comportamenti specifici per AuthUser
    public AuthUserService(
        IRepository<AuthUser, Guid> repository,
        IMapper<AuthUser, AuthUserDto> databaseToDtoMapper,
        IMapper<AuthUserDto, AuthUser> dtoToDatabaseMapper,
        ResponseMessageOption messages)
        : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
    {
        // Eredita tutte le funzionalità base e può aggiungerne di specifiche
    }

    // Potrebbe aggiungere metodi specifici per AuthUser
    // public async Task<BusinessResponse<AuthUserDto>> GetByEmailAsync(string email)
    // {
    //     return await FindAsync(u => u.Email == email);
    // }
}
```

### Manager Decorator Pattern

```csharp
// DietManager decora BaseManager con funzionalità specifiche per Diet
internal class DietManager : BaseManager<CreateDietRequest>, IManager<DietDto, CreateDietRequest, int>
{
    private readonly IService<DietDto, Diet, int> _dietService;
    private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
    // Altri servizi decorati...

    // Decora BaseManager con logica business specifica per Diet
    public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
    {
        // Utilizza i metodi template della classe base
        var validationResult = ValidateAndGetUserId(request, userIdClaim);

        // Aggiunge decorazioni specifiche per Diet:
        // - Validazione business rules
        // - Controllo duplicati
        // - Mapping specifico
        // - Audit logging

        if (validationResult is null)
        {
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
        }

        var userId = (Guid)validationResult;

        // Decorazione: verifica esistenza utente
        var userRes = await _userService.GetByIdAsync(userId);
        if (userRes.Data is null)
        {
            return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
        }

        // Decorazione: controllo business rule (no duplicati)
        var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);
        if (existingDietRes.Data?.ToList().Count != 0)
        {
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.DietAlreadyExists);
        }

        // Decorazione: mapping chain e audit
        var createDto = _createRequestToCreateDtoMapper.Map(request);
        createDto.UserId = userId;
        var dietDto = _createDtoToDietDtoMapper.Map(createDto);
        dietDto.CreatedAt = DateTime.UtcNow;
        dietDto.UpdatedAt = dietDto.CreatedAt;

        return await _dietService.CreateAsync(dietDto);
    }
}
```

### Authentication Handler Decorator

```csharp
// CustomJwtAuthenticationHandler decora AuthenticationHandler con logica JWT custom
public class CustomJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Decora il componente base con:
    // - Logica di validazione JWT custom
    // - Gestione chiavi pubbliche
    // - Mapping custom dei claims

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Decorazione del processo di autenticazione base con:

        // 1. Estrazione token custom
        var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return AuthenticateResult.Fail("Missing or invalid authorization header.");
        }

        // 2. Validazione con chiavi pubbliche custom
        var publicKeysResponse = await _publicKeyService.GetAsync();
        var validationKeys = _keyPairMapper.Map(publicKeysResponse.Data);

        // 3. Parametri di validazione custom
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

        // 4. Decorazione del risultato
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        return AuthenticateResult.Success(new AuthenticationTicket(principal, "CustomJwt"));
    }
}
```

### Database Context Decorator

```csharp
// MyDietAuthDb decora DbContext con interfaccia custom
internal class MyDietAuthDb : IDatabase<MyDietAuthDbContext>
{
    private readonly MyDietAuthDbContext _context;

    public MyDietAuthDb(MyDietAuthDbContext context)
    {
        _context = context;
    }

    // Decora DbContext con interfaccia semplificata
    public MyDietAuthDbContext Context { get => _context; }

    // Potrebbe aggiungere decorazioni come:
    // - Audit logging
    // - Transaction management
    // - Connection pooling custom
    // - Cache management
}
```

### Repository Decorator Pattern

```csharp
// Tutti i repository decorano AGenericRepository con specificità del dominio
internal class DietRepository : AGenericRepository<MyDietCoreDbContext, Diet, int>
{
    public DietRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessageOption messages)
        : base(db, messages)
    {
        // Eredita tutte le operazioni CRUD base
        // Può aggiungere decorazioni specifiche per Diet:
        // - Query ottimizzate
        // - Validazioni specifiche
        // - Audit logging
        // - Cache management
    }

    // Esempio di decorazione specifica
    // public async Task<BusinessResponse<IEnumerable<Diet>>> GetByUserIdAsync(Guid userId)
    // {
    //     return await FindAsync(d => d.UserId == userId);
    // }
}
```

## Chain of Decorators

La soluzione implementa catene di decoratori:

```
Controller -> Manager (decorato) -> Service (decorato) -> Repository (decorato) -> DbContext (decorato)
```

Ogni livello aggiunge decorazioni specifiche:

- **Manager**: Business rules, validazione, orchestrazione
- **Service**: Mapping, gestione errori, transazioni
- **Repository**: Ottimizzazioni query, cache
- **DbContext**: Audit, connection management

## Vantaggi dell'Implementazione Attuale

1. **Layered Enhancement**: Ogni livello aggiunge funzionalità specifiche
2. **Separation of Concerns**: Decorazioni organizzate per responsabilità
3. **Composability**: Decoratori possono essere combinati facilmente
4. **Reusability**: Decoratori base riutilizzabili per diverse entità
5. **Extensibility**: Facile aggiungere nuove decorazioni

## Decorazioni Implementate

1. **Validation Decorators**: Validazione input nei manager
2. **Business Rule Decorators**: Controlli business nei manager
3. **Mapping Decorators**: Conversioni tra livelli nei servizi
4. **Audit Decorators**: Timestamp e tracking nei servizi
5. **Security Decorators**: Autenticazione e autorizzazione
6. **Error Handling Decorators**: Gestione errori standardizzata

## Vantaggi del Pattern

- **Flessibilità**: Aggiunta dinamica di funzionalità
- **Single Responsibility**: Ogni decoratore ha una responsabilità specifica
- **Open/Closed Principle**: Estensione senza modifiche
- **Composizione**: Alternative all'ereditarietà

## Applicazione Corretta

Il pattern è implementato correttamente attraverso:

- Ereditarietà da classi base generiche
- Aggiunta di funzionalità specifiche per dominio
- Mantenimento dell'interfaccia originale
- Composizione di decoratori in catene logiche

## Pattern Correlati

- Spesso utilizzato insieme al **Strategy Pattern** per decorare comportamenti
- Si integra con **Facade Pattern** per nascondere la complessità delle decorazioni
