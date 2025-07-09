# Template Method Pattern

## Descrizione

Il Template Method Pattern definisce lo scheletro di un algoritmo in una classe base, delegando alcuni passaggi alle sottoclassi. Le sottoclassi possono ridefinire certi passaggi dell'algoritmo senza cambiarne la struttura complessiva.

## File Coinvolti

### Classe Base (BaseUtility)

- `AGenericRepository<TContext, TEntity, TKey>` - Template per repository
- `AGenericService<TDto, TEntity, TKey>` - Template per servizi

### Implementazioni Template - Managers

- `src/Businesses/MyDiet.Core.Business/Managers/BaseManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/DietManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/PlanManager.cs`

### Implementazioni Template - Repository

- `src/Infrastructures/MyDiet.Auth.Infrastructure/Repositories/AuthUserRepository.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/Repositories/CoreUserRepository.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/Repositories/DietRepository.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/Repositories/PlanRepository.cs`

### Implementazioni Template - Services

- `src/Businesses/MyDiet.Auth.Business/Services/AuthUserService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/CoreUserService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/DietService.cs`
- `src/Businesses/MyDiet.Core.Business/Services/PlanService.cs`

## Implementazione

### BaseManager Template

```csharp
internal abstract class BaseManager<TRequest> : IValidationManager<TRequest>
    where TRequest : class
{
    // Template methods definiti nella classe base
    public Guid? ValidateUserClaim(Claim? claim)
    {
        if (claim is null || !Guid.TryParse(claim.Value, out var userId))
        {
            return null;
        }
        return userId;
    }

    public TRequest? ValidateRequest(TRequest request)
    {
        if (request is null)
        {
            return null;
        }
        return request;
    }

    // Template method che combina le validazioni
    public virtual Guid? ValidateAndGetUserId(TRequest request, Claim? claim)
    {
        var validRequest = ValidateRequest(request);
        if (validRequest is null)
        {
            return null;
        }
        var userId = ValidateUserClaim(claim);
        if (userId is null)
        {
            return null;
        }
        return userId;
    }
}
```

### Implementazione Concreta - DietManager

```csharp
internal class DietManager : BaseManager<CreateDietRequest>, IManager<DietDto, CreateDietRequest, int>
{
    // La classe eredita i template methods da BaseManager
    // e implementa i metodi specifici per Diet

    public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
    {
        // Utilizza il template method ereditato
        var validationResult = ValidateAndGetUserId(request, userIdClaim);

        if (validationResult is null)
        {
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
        }

        // Logica specifica per la creazione di Diet
        // ...
    }
}
```

### Repository Template (BaseUtility)

```csharp
// AGenericRepository fornisce template methods per operazioni CRUD
public abstract class AGenericRepository<TContext, TEntity, TKey> : IRepository<TEntity, TKey>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    // Template methods per CRUD operations
    // GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync, FindAsync
}
```

### Implementazione Repository Concreta

```csharp
internal class DietRepository : AGenericRepository<MyDietCoreDbContext, Diet, int>
{
    public DietRepository(IDatabase<MyDietCoreDbContext> db, ResponseMessageOption messages)
        : base(db, messages)
    {
        // Eredita tutti i template methods da AGenericRepository
        // Può sovrascrivere metodi specifici se necessario
    }
}
```

### Service Template (BaseUtility)

```csharp
// AGenericService fornisce template methods per operazioni business
public abstract class AGenericService<TDto, TEntity, TKey> : IService<TDto, TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TDto : class
    where TKey : notnull
{
    // Template methods che coordinano repository e mapping
    // GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync, FindAsync
}
```

## Vantaggi dell'Implementazione Attuale

1. **Riutilizzo del codice**: Logica comune condivisa tra implementazioni
2. **Consistenza**: Comportamento uniforme attraverso tutte le implementazioni
3. **Manutenibilità**: Modifiche ai template methods si riflettono su tutte le implementazioni
4. **Estensibilità**: Nuove implementazioni possono facilmente estendere i template

## Vantaggi del Pattern

- **Eliminazione duplicazione**: Codice comune definito una sola volta
- **Controllo del flusso**: La classe base controlla l'algoritmo principale
- **Estensibilità**: Le sottoclassi possono personalizzare comportamenti specifici
- **Consistenza**: Garantisce comportamento uniforme

## Applicazione Corretta

Il pattern è implementato correttamente:

- Template methods ben definiti nelle classi base
- Implementazioni concrete che utilizzano appropriatamente l'ereditarietà
- Logica comune astratta nelle classi base
- Comportamenti specifici implementati nelle classi derivate
- Utilizzo di generics per massimizzare il riutilizzo

## Pattern Correlati

- Questo pattern lavora bene con **Strategy Pattern** per parti dell'algoritmo che possono variare
- Si integra con **Factory Method Pattern** quando i template methods devono creare oggetti
