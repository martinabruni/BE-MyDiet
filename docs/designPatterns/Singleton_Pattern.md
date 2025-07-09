# Singleton Pattern

## Descrizione

Il Singleton Pattern garantisce che una classe abbia una sola istanza e fornisce un punto di accesso globale a tale istanza. Questo pattern è utile quando è necessario coordinare azioni attraverso il sistema utilizzando un singolo oggetto.

## File Coinvolti

### Configuration Options (Singletons via DI)

- `src/Domains/MyDiet.Auth.Domain/Options/KeyOption.cs`
- `src/Domains/MyDiet.Auth.Domain/Options/JsonWebKeyOption.cs`
- `src/Domains/MyDiet.Auth.Domain/Options/TokenOption.cs`
- `src/Domains/MyDiet.Auth.Domain/Options/VaultMessageOption.cs`
- `src/Domains/MyDiet.Auth.Domain/Options/AuthManagerMessageOption.cs`
- `src/Domains/MyDiet.Auth.Domain/Options/KeyPairMessageOption.cs`
- `src/Domains/MyDiet.Core.Domain/Options/DietMessageOption.cs`
- `src/Domains/MyDiet.Core.Domain/Options/PlanMessageOption.cs`

### Singleton Services

- `SecretClient` (Azure Key Vault Client)
- `KeyPair` (Cryptographic Key Pair)
- `ByteArrayBase64Converter`
- `ResponseMessageOption` (BaseUtility)

### Configuration Files

- `src/Infrastructures/MyDiet.Auth.Infrastructure/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Businesses/MyDiet.Auth.Business/ExtensionMethods/ServiceCollectionExtension.cs`
- `src/Businesses/MyDiet.Core.Business/ExtensionMethods/ServiceCollectionExtension.cs`

## Implementazione

### Configuration Singletons

```csharp
// Registration in Auth Infrastructure
public static IServiceCollection AddKeyPairInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    var jwtSection = configuration.GetSection("Jwt");
    var algorithm = jwtSection["Jwk:Alg"];

    // Singleton: Azure Secret Client - una sola istanza per app
    services.AddSingleton(new SecretClient(new Uri(configuration["Vault:Uri"]), new DefaultAzureCredential()));

    // Singleton: Key Options - configurazione immutabile
    services.AddSingleton(new KeyOption
    {
        PrivateKeyName = configuration["Key:PrivateKeyName"],
        KeySize = int.Parse(configuration["Key:KeySize"]),
    });

    // Singleton: JsonWebKey Options - configurazione immutabile
    services.AddSingleton(new JsonWebKeyOption
    {
        Kty = jwtSection["Jwk:Kty"],
        Use = jwtSection["Jwk:Use"],
        Alg = algorithm
    });

    // Singleton: Message Options - configurazione statica
    services.AddSingleton<VaultMessageOption>();

    // Singleton: KeyPair - istanza unica per l'applicazione
    services.AddSingleton<KeyPair>();

    return services;
}

// Registration in Auth Infrastructure
public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    // Singleton: Response Messages - configurazione globale
    services.AddSingleton<ResponseMessageOption>();

    // Singleton: Token Options - configurazione immutabile
    services.AddSingleton(new TokenOption
    {
        Algorithm = tokenSection["Algorithm"],
        Audience = tokenSection["Audience"],
        ExpiryMinutes = int.Parse(tokenSection["ExpiryMinutes"]),
        Issuer = tokenSection["Issuer"]
    });

    return services;
}
```

### Business Singletons

```csharp
// Auth Business Singletons
public static IServiceCollection AddKeyPairBusiness(this IServiceCollection services)
{
    // Singleton: Converter - utility stateless
    services.AddSingleton<ByteArrayBase64Converter>();

    // Singleton: Message Options - configurazione
    services.AddSingleton<KeyPairMessageOption>();

    return services;
}

public static IServiceCollection AddAuthBusiness(this IServiceCollection services)
{
    // Singleton: Message Options - configurazione globale
    services.AddSingleton<AuthManagerMessageOption>();

    return services;
}

// Core Business Singletons
public static IServiceCollection AddCoreBusiness(this IServiceCollection services)
{
    // Singleton: Message Options per ogni dominio
    services.AddSingleton<DietMessageOption>();
    services.AddSingleton<PlanMessageOption>();

    return services;
}
```

### Option Classes (Singleton Pattern via DI)

```csharp
// TokenOption - Singleton che contiene configurazione token
public class TokenOption
{
    public string Algorithm { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
    public string Issuer { get; set; }
}

// KeyOption - Singleton per configurazione chiavi
public class KeyOption
{
    public string PrivateKeyName { get; set; }
    public int KeySize { get; set; }
}

// JsonWebKeyOption - Singleton per configurazione JWK
public class JsonWebKeyOption
{
    public string Kty { get; set; }
    public string Use { get; set; }
    public string Alg { get; set; }
}
```

### Message Options Singletons

```csharp
// AuthManagerMessageOption - Singleton per messaggi di AuthManager
public class AuthManagerMessageOption
{
    public string InvalidRequest { get; set; } = "Invalid request";
    public string UserAlreadyExists { get; set; } = "User already exists";
    public string UserCreatedSuccessfully { get; set; } = "User created successfully";
    public string ErrorCreatingEntity { get; set; } = "Error creating entity";
    public string ErrorRetrievingEntities { get; set; } = "Error retrieving entities";
}

// DietMessageOption - Singleton per messaggi di DietManager
public class DietMessageOption
{
    public string InvalidRequest { get; set; } = "Invalid request";
    public string EntityNotFound { get; set; } = "Entity not found";
    public string DietAlreadyExists { get; set; } = "Diet already exists";
    public string ErrorCreatingEntity { get; set; } = "Error creating entity";
    public string ErrorUpdatingEntity { get; set; } = "Error updating entity";
    public string ErrorRetrievingEntities { get; set; } = "Error retrieving entities";
}
```

### Singleton Usage Example

```csharp
// Usage nel DietManager - riceve singleton via DI
internal class DietManager : BaseManager<CreateDietRequest>
{
    private readonly DietMessageOption _responseMessageOption; // Singleton injected

    public DietManager(
        IService<CoreUserDto, CoreUser, Guid> userService,
        IService<DietDto, Diet, int> dietService,
        IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper,
        IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper,
        DietMessageOption responseMessageOption) // Singleton injection
    {
        _responseMessageOption = responseMessageOption;
        // Stessa istanza in tutti i DietManager
    }

    public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
    {
        if (validationResult is null)
        {
            // Utilizza il singleton per messaggi consistenti
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
        }
    }
}
```

### Azure Services Singleton

```csharp
// SecretClient - Singleton per accesso Azure Key Vault
public class PrivateKeyRepository : IVaultRepository<KeyVaultSecret>
{
    private readonly SecretClient _secretClient; // Singleton shared

    public PrivateKeyRepository(SecretClient secretClient, KeyOption keyOption, VaultMessageOption messages)
    {
        _secretClient = secretClient; // Stessa istanza in tutta l'app
        _keyOption = keyOption;
        _messages = messages;
    }

    public async Task<BusinessResponse<KeyVaultSecret>> GetAsync()
    {
        try
        {
            // Utilizza il singleton SecretClient
            var secret = await _secretClient.GetSecretAsync(_keyOption.PrivateKeyName);
            return BusinessResponse<KeyVaultSecret>.Ok(secret.Value, _messages.SecretRetrieved);
        }
        catch (Exception ex)
        {
            return BusinessResponse<KeyVaultSecret>.InternalServerError($"{_messages.ErrorRetrievingSecret}: {ex.Message}");
        }
    }
}
```

## Vantaggi dell'Implementazione Attuale

1. **Configurazione Centrale**: Options come singleton garantiscono configurazione consistente
2. **Efficienza Memoria**: Una sola istanza per configurazioni immutabili
3. **Thread Safety**: DI container garantisce thread safety
4. **Lifetime Management**: Container gestisce il ciclo di vita
5. **Dependency Injection**: Integrazione pulita con il sistema DI

## Tipi di Singleton Implementati

### 1. Configuration Singletons

- **TokenOption**: Configurazione JWT immutabile
- **KeyOption**: Configurazione chiavi crittografiche
- **JsonWebKeyOption**: Configurazione JWK

### 2. Message Singletons

- **AuthManagerMessageOption**: Messaggi per autenticazione
- **DietMessageOption**: Messaggi per gestione diete
- **PlanMessageOption**: Messaggi per gestione piani

### 3. Service Singletons

- **SecretClient**: Client Azure Key Vault
- **ByteArrayBase64Converter**: Utility converter
- **KeyPair**: Coppia chiavi crittografiche

### 4. Infrastructure Singletons

- **ResponseMessageOption**: Messaggi base (BaseUtility)
- **VaultMessageOption**: Messaggi per Key Vault

## Vantaggi del Pattern

- **Controllo Istanze**: Garantisce una sola istanza
- **Accesso Globale**: Punto di accesso globale
- **Lazy Loading**: Creazione solo quando necessario (tramite DI)
- **Thread Safety**: Gestito dal DI container
- **Memory Efficiency**: Condivisione di istanze costose

## Applicazione Corretta

Il pattern è implementato correttamente attraverso:

- Dependency Injection container che garantisce singola istanza
- Registrazione appropriata con AddSingleton
- Utilizzo per configurazioni immutabili e servizi costosi
- Thread safety gestita dal framework
- Lifetime appropriato per ogni tipo di singleton

## Motivazioni per l'Uso di Singleton

1. **Configuration Objects**: Immutabili e condivisi
2. **Expensive Resources**: SecretClient con connessioni Azure
3. **Message Constants**: Messaggi statici condivisi
4. **Cryptographic Objects**: KeyPair per sicurezza
5. **Utility Classes**: Converter stateless

## Pattern Correlati

- Spesso utilizzato insieme al **Factory Pattern** per creare istanze singleton
- Si integra con **Dependency Injection** per gestione del lifetime

## Note sulla Thread Safety

Tutti i singleton sono thread-safe grazie al DI container di ASP.NET Core che:

- Garantisce creazione thread-safe
- Utilizza lazy loading
- Gestisce accessi concorrenti
- Previene race conditions durante la creazione
