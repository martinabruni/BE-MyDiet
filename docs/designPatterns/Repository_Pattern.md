# Repository Pattern

## Descrizione

Il Repository Pattern fornisce un livello di astrazione tra la logica di business e il livello di accesso ai dati. Questo pattern incapsula la logica necessaria per accedere alle fonti di dati, centralizzando le funzionalità comuni di accesso ai dati fornendo una migliore manutenibilità e disaccoppiando l'infrastruttura o la tecnologia utilizzata per accedere ai database dal livello del dominio del modello.

## File Coinvolti

### Interfacce di Base (BaseUtility)

- `IRepository<TEntity, TKey>` - Interfaccia base per repository generici
- `AGenericRepository<TContext, TEntity, TKey>` - Implementazione base astratta

### Repository Concreti - Auth Domain

- `src/Infrastructures/MyDiet.Auth.Infrastructure/Repositories/AuthUserRepository.cs`
- `src/Infrastructures/MyDiet.Auth.Infrastructure/Repositories/PrivateKeyRepository.cs`
- `src/Domains/MyDiet.Auth.Domain/Repositories/IVaultRepository.cs`

### Repository Concreti - Core Domain

- `src/Infrastructures/MyDiet.Core.Infrastructure/Repositories/CoreUserRepository.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/Repositories/DietRepository.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/Repositories/PlanRepository.cs`

### Configurazione Dependency Injection

- `src/Infrastructures/MyDiet.Auth.Infrastructure/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Infrastructures/MyDiet.Core.Infrastructure/ExtensionMethods/ServiceCollectionExtension.cs`

## Implementazione

### Esempio - AuthUserRepository

```csharp
internal class AuthUserRepository : AGenericRepository<MyDietAuthDbContext, AuthUser, Guid>
{
    public AuthUserRepository(IDatabase<MyDietAuthDbContext> db, ResponseMessageOption messages)
        : base(db, messages)
    {
    }
}
```

### Registrazione DI - Auth Infrastructure

```csharp
services.AddScoped<IRepository<AuthUser, Guid>, AuthUserRepository>();
```

### Registrazione DI - Core Infrastructure

```csharp
services.AddScoped<IRepository<CoreUser, Guid>, CoreUserRepository>();
services.AddScoped<IRepository<Diet, int>, DietRepository>();
services.AddScoped<IRepository<Plan, int>, PlanRepository>();
```

## Vantaggi dell'Implementazione Attuale

1. **Separazione delle responsabilità**: I repository gestiscono solo l'accesso ai dati
2. **Testabilità**: Possibilità di mock dei repository per i test unitari
3. **Riusabilità**: Utilizzo di un repository base generico
4. **Centralizzazione**: Tutte le operazioni CRUD sono centralizzate
5. **Astrazione**: Il business layer non dipende direttamente dal DbContext

## Applicazione Corretta

Il pattern è implementato correttamente seguendo le best practices:

- Interfacce chiaramente definite
- Implementazioni concrete specifiche per dominio
- Registrazione tramite Dependency Injection
- Utilizzo di generics per ridurre la duplicazione del codice
- Separazione tra domini (Auth e Core)
