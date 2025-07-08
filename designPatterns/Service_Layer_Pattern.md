# Service Layer Pattern

## Descrizione

Il Service Layer Pattern definisce un confine dell'applicazione con un livello di servizi che stabilisce un insieme di operazioni disponibili e coordina la risposta dell'applicazione in ogni operazione. Questo pattern incapsula la logica di business dell'applicazione, controllando le transazioni e coordinando le risposte nell'implementazione delle operazioni.

## File Coinvolti

### Interfacce di Base (BaseUtility)

- `IService<TDto, TEntity, TKey>` - Interfaccia base per servizi generici
- `AGenericService<TDto, TEntity, TKey>` - Implementazione base astratta

### Servizi - Auth Domain

- `src/Businesses/MyDiet.Auth.Business/Services/AuthUserService.cs`
- `src/Businesses/MyDiet.Auth.Business/Services/TokenService.cs`
- `src/Businesses/MyDiet.Auth.Business/Services/PublicKeyService.cs`
- `src/Businesses/MyDiet.Auth.Business/Services/PrivateKeyService.cs`
- `src/Domains/MyDiet.Auth.Domain/Services/ITokenService.cs`
- `src/Domains/MyDiet.Auth.Domain/Services/IVaultService.cs`

### Servizi - Core Domain

- `src/Businesses/MyDiet.Core.Business/Services/CoreUserService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/DietService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/PlanService.cs`

### Configurazione Dependency Injection

- `src/Businesses/MyDiet.Auth.Business/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Businesses/MyDiet.Core.Business/ExtensionMethods/ServiceCollectionExtension.cs`

## Implementazione

### Esempio - AuthUserService

```csharp
internal class AuthUserService : AGenericService<AuthUserDto, AuthUser, Guid>
{
    public AuthUserService(
        IRepository<AuthUser, Guid> repository,
        IMapper<AuthUser, AuthUserDto> databaseToDtoMapper,
        IMapper<AuthUserDto, AuthUser> dtoToDatabaseMapper,
        ResponseMessageOption messages)
        : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
    {
    }
}
```

### Esempio - DietService

```csharp
internal class DietService : AGenericService<DietDto, Diet, int>
{
    public DietService(
        IRepository<Diet, int> repository,
        IMapper<Diet, DietDto> databaseToDtoMapper,
        IMapper<DietDto, Diet> dtoToDatabaseMapper,
        ResponseMessageOption messages)
        : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
    {
    }
}
```

### Registrazione DI - Auth Business

```csharp
services.AddScoped<IService<AuthUserDto, AuthUser, Guid>, AuthUserService>();
services.AddScoped<ITokenService, TokenService>();
```

### Registrazione DI - Core Business

```csharp
services.AddScoped<IService<DietDto, Diet, int>, DietService>();
services.AddScoped<IService<PlanDto, Plan, int>, PlanService>();
services.AddScoped<IService<CoreUserDto, CoreUser, Guid>, CoreUserService>();
```

## Vantaggi dell'Implementazione Attuale

1. **Astrazione dei dati**: I servizi lavorano con DTO invece che con entità del database
2. **Logica di business centralizzata**: Le operazioni CRUD sono incapsulate nei servizi
3. **Riusabilità**: Utilizzo di un servizio base generico
4. **Mapping automatico**: Conversione automatica tra entità e DTO
5. **Gestione errori**: Standardizzazione delle risposte attraverso `BusinessResponse<T>`

## Vantaggi del Pattern

- **Disaccoppiamento**: Il livello di presentazione non conosce i dettagli del repository
- **Transazioni**: Coordinamento di operazioni multiple
- **Validazione**: Centralizzazione della logica di validazione
- **Sicurezza**: Controllo degli accessi centralizzato

## Applicazione Corretta

Il pattern è implementato correttamente:

- Chiara separazione tra servizi e repository
- Utilizzo di DTO per il trasferimento dati
- Gestione standardizzata delle risposte
- Dependency Injection configurata correttamente
- Servizi specifici per domini diversi (Auth e Core)
