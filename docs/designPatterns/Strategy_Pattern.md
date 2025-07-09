# Strategy Pattern

## Descrizione

Il Strategy Pattern definisce una famiglia di algoritmi, li incapsula e li rende intercambiabili. Questo pattern permette all'algoritmo di variare indipendentemente dai client che lo utilizzano.

## File Coinvolti

### Mapper Strategies

- `src/Businesses/MyDiet.Core.Business/Mappers/DietMapper.cs`
- `src/Businesses/MyDiet.Core.Business/Mappers/PlanMapper.cs`
- `src/Businesses/MyDiet.Core.Business/Mappers/CoreUserMapper.cs`
- `src/Businesses/MyDiet.Auth.Business/Mappers/AuthUserMapper.cs`
- `src/Businesses/MyDiet.Auth.Business/Mappers/TokenMapper.cs`
- `src/Businesses/MyDiet.Auth.Business/Mappers/KeyPairMapper.cs`
- `src/Businesses/MyDiet.Auth.Business/Mappers/ClaimMapper.cs`

### Service Strategies

- `src/Businesses/MyDiet.Auth.Business/Services/TokenService.cs`
- `src/Businesses/MyDiet.Auth.Business/Services/PublicKeyService.cs`
- `src/Businesses/MyDiet.Auth.Business/Services/PrivateKeyService.cs`

### Strategy Interfaces (BaseUtility)

- `IMapper<TSource, TDestination>` - Interfaccia per strategie di mapping
- `IService<TDto, TEntity, TKey>` - Interfaccia per strategie di servizio

## Implementazione

### Mapper Strategy - DietMapper

```csharp
// Implementa multiple strategie di mapping per Diet
internal class DietMapper :
    IMapper<Diet, DietDto>,           // Strategia Entity -> DTO
    IMapper<DietDto, Diet>,           // Strategia DTO -> Entity
    IMapper<CreateDietRequest, CreateDietDto>,  // Strategia Request -> CreateDTO
    IMapper<CreateDietDto, DietDto>   // Strategia CreateDTO -> DTO
{
    // Strategia: Entity -> DTO
    public DietDto Map(Diet input)
    {
        return new DietDto
        {
            Id = input.Id,
            UserId = input.UserId,
            Name = input.Name,
            CreatedAt = input.CreatedAt,
            UpdatedAt = input.UpdatedAt
        };
    }

    // Strategia: DTO -> Entity
    public Diet Map(DietDto input)
    {
        return new Diet
        {
            Id = input.Id,
            UserId = input.UserId,
            Name = input.Name,
            CreatedAt = input.CreatedAt,
            UpdatedAt = input.UpdatedAt
        };
    }

    // Strategia: Request -> CreateDTO
    public CreateDietDto Map(CreateDietRequest input)
    {
        return new CreateDietDto
        {
            Name = input.Name
            // UserId sarà impostato dal manager
        };
    }

    // Strategia: CreateDTO -> DTO
    public DietDto Map(CreateDietDto input)
    {
        return new DietDto
        {
            Id = 0, // ID autogenerato
            UserId = input.UserId,
            Name = input.Name,
        };
    }
}
```

### AuthUserMapper Strategy

```csharp
// Implementa multiple strategie di mapping per AuthUser
internal class AuthUserMapper :
    IMapper<AuthUser, AuthUserDto>,
    IMapper<AuthUserDto, AuthUser>,
    IMapper<UserRegistrationRequest, AuthUserDto>,
    IMapper<AuthUserDto, UserRegistrationResponse>,
    IMapper<AuthUserDto, UserClaims>
{
    // Diverse strategie di mapping per diversi scenari
    // - Database mapping
    // - Request mapping
    // - Response mapping
    // - Claims mapping
}
```

### Service Strategy Utilization - DietManager

```csharp
public class DietManager
{
    // Utilizza diverse strategie di mapping
    private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
    private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;

    public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
    {
        // Strategia 1: Request -> CreateDTO
        var createDto = _createRequestToCreateDtoMapper.Map(request);

        // Strategia 2: CreateDTO -> DTO
        var dietDto = _createDtoToDietDtoMapper.Map(createDto);

        return await _dietService.CreateAsync(dietDto);
    }

    public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
    {
        // Stesso context, stesse strategie ma workflow diverso
        var createDto = _createRequestToCreateDtoMapper.Map(request);
        var newDto = _createDtoToDietDtoMapper.Map(createDto);

        // Logica specifica per update
        newDto.Id = id;
        newDto.UpdatedAt = DateTime.UtcNow;

        return await _dietService.UpdateAsync(newDto);
    }
}
```

### Token Strategy - Multiple Implementations

```csharp
// Diverse strategie per gestione chiavi
public interface IVaultService<TData>
{
    Task<BusinessResponse<TData>> GetAsync();
}

// Strategia per chiavi private
internal class PrivateKeyService : IVaultService<KeyVaultSecret>
{
    // Algoritmo specifico per recupero chiavi private
}

// Strategia per chiavi pubbliche
internal class PublicKeyService : IVaultService<JsonWebKeySetDto>
{
    // Algoritmo specifico per generazione chiavi pubbliche
}
```

### Strategy Configuration

```csharp
// Registrazione delle diverse strategie
public static IServiceCollection AddCoreBusiness(this IServiceCollection services)
{
    // Registrazione strategie di mapping
    services.AddScoped<IMapper<Diet, DietDto>, DietMapper>();
    services.AddScoped<IMapper<DietDto, Diet>, DietMapper>();
    services.AddScoped<IMapper<CreateDietRequest, CreateDietDto>, DietMapper>();
    services.AddScoped<IMapper<CreateDietDto, DietDto>, DietMapper>();

    // Stessa classe implementa multiple strategie
    services.AddScoped<IMapper<Plan, PlanDto>, PlanMapper>();
    services.AddScoped<IMapper<PlanDto, Plan>, PlanMapper>();
    services.AddScoped<IMapper<CreatePlanRequest, PlanDto>, PlanMapper>();

    return services;
}
```

## Vantaggi dell'Implementazione Attuale

1. **Flessibilità**: Diverse strategie di mapping per diversi scenari
2. **Riusabilità**: Stessa interfaccia per algoritmi diversi
3. **Intercambiabilità**: Possibilità di sostituire strategie via DI
4. **Separazione delle responsabilità**: Ogni strategia gestisce un solo tipo di conversione
5. **Estensibilità**: Facile aggiungere nuove strategie

## Scenari di Utilizzo

1. **Mapping Strategies**: Diverse strategie per convertire tra tipi
2. **Service Strategies**: Diverse implementazioni per servizi simili
3. **Validation Strategies**: Diverse regole di validazione
4. **Authentication Strategies**: Diverse modalità di autenticazione

## Vantaggi del Pattern

- **Eliminazione condizionali**: No più if/switch per scegliere algoritmi
- **Estensibilità**: Facile aggiungere nuovi algoritmi
- **Testabilità**: Ogni strategia può essere testata indipendentemente
- **Single Responsibility**: Ogni strategia ha una singola responsabilità

## Applicazione Corretta

Il pattern è implementato correttamente:

- Interfacce comuni per famiglie di algoritmi
- Implementazioni concrete per strategie specifiche
- Configurazione tramite Dependency Injection
- Utilizzo polimorfismo per intercambiabilità

## Possibili Miglioramenti

Alcuni mapper implementano multiple interfacce nella stessa classe. Per una migliore aderenza al pattern, si potrebbe considerare:

```csharp
// Invece di una classe con multiple implementazioni
internal class DietMapper : IMapper<Diet, DietDto>, IMapper<DietDto, Diet>

// Strategie separate più pure
internal class DietToDtoMapper : IMapper<Diet, DietDto>
internal class DtoToDietMapper : IMapper<DietDto, Diet>
```

Tuttavia, l'implementazione attuale è pragmatica e funzionale per il contesto del progetto.

## Pattern Correlati

- Spesso utilizzato con **Factory Pattern** per creare le strategie appropriate
- Si integra bene con **Template Method Pattern** quando le strategie condividono strutture simili
