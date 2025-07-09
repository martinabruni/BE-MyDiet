# Command Pattern

## Descrizione

Il Command Pattern incapsula una richiesta come un oggetto, permettendo così di parametrizzare i client con diverse richieste, accodare o registrare le richieste e supportare operazioni di annullamento. Questo pattern disaccoppia l'oggetto che invoca l'operazione dall'oggetto che la esegue.

## File Coinvolti

### Request Objects (Commands)

- `src/Domains/MyDiet.Core.Domain/Dtos/Requests/CreateDietRequest.cs`
- `src/Domains/MyDiet.Core.Domain/Dtos/Requests/CreatePlanRequest.cs`
- `src/Domains/MyDiet.Auth.Domain/Dtos/Requests/UserRegistrationRequest.cs`
- `src/Domains/MyDiet.Auth.Domain/Dtos/Requests/UserLoginRequest.cs`

### Command Handlers (Invokers)

- `src/Businesses/MyDiet.Core.Business/Managers/DietManager.cs`
- `src/Businesses/MyDiet.Core.Business/Managers/PlanManager.cs`
- `src/Businesses/MyDiet.Auth.Business/Managers/AuthManager.cs`

### Command Interfaces

- `src/Domains/MyDiet.Core.Domain/Managers/IManager.cs`
- `src/Domains/MyDiet.Auth.Domain/Managers/IAuthManager.cs`

### Controllers (Client)

- `src/Apis/MyDiet.Shared.Api/Controllers/Core/`
- `src/Apis/MyDiet.Shared.Api/Controllers/Auth/`

## Implementazione

### Command Objects

```csharp
// CreateDietRequest è un Command Object
public class CreateDietRequest
{
    public string Name { get; set; }
    // Incapsula tutti i parametri necessari per creare una dieta
}

// CreatePlanRequest è un Command Object
public class CreatePlanRequest
{
    public int DietId { get; set; }
    public string Name { get; set; }
    // Incapsula tutti i parametri necessari per creare un piano
}

// UserRegistrationRequest è un Command Object
public class UserRegistrationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    // Incapsula tutti i parametri per la registrazione
}
```

### Command Interface

```csharp
// IManager definisce l'interfaccia per i Command Handlers
public interface IManager<TResponse, TRequest, TKey>
    where TResponse : class
    where TRequest : class  // TRequest è il Command Object
    where TKey : notnull
{
    // Command methods che accettano Command Objects
    Task<BusinessResponse<TResponse>> GetByIdAsync(TKey id, Claim? userIdClaim);
    Task<BusinessResponse<TResponse>> CreateAsync(TRequest request, Claim? userIdClaim);
    Task<BusinessResponse<TResponse>> UpdateAsync(TRequest request, TKey id, Claim? userIdClaim);
    Task<BusinessResponse<TResponse>> DeleteAsync(TKey id, Claim? userIdClaim);
    Task<BusinessResponse<IEnumerable<TResponse>>> GetByUserIdAsync(Claim? userIdClaim);
}
```

### Command Handler Implementation

```csharp
// DietManager è un Command Handler
internal class DietManager : BaseManager<CreateDietRequest>, IManager<DietDto, CreateDietRequest, int>
{
    // Execute command: CreateDietRequest
    public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
    {
        // 1. Validazione del command
        var validationResult = ValidateAndGetUserId(request, userIdClaim);
        if (validationResult is null)
        {
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
        }

        // 2. Esecuzione del command
        var userId = (Guid)validationResult;

        // 3. Business logic per l'esecuzione
        var userRes = await _userService.GetByIdAsync(userId);
        if (userRes.Data is null)
        {
            return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
        }

        // 4. Controlli di business rules
        var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);
        if (existingDietRes.Data?.ToList().Count != 0)
        {
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.DietAlreadyExists);
        }

        // 5. Mapping del command
        var createDto = _createRequestToCreateDtoMapper.Map(request);
        createDto.UserId = userId;
        var dietDto = _createDtoToDietDtoMapper.Map(createDto);

        // 6. Audit e timestamp
        dietDto.CreatedAt = DateTime.UtcNow;
        dietDto.UpdatedAt = dietDto.CreatedAt;

        // 7. Esecuzione finale del command
        return await _dietService.CreateAsync(dietDto);
    }

    // Execute command: UpdateDietRequest (stesso CreateDietRequest ma semantica diversa)
    public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
    {
        // Command handler per update con logica specifica
        var validRequest = ValidateAndGetUserId(request, userIdClaim);
        if (validRequest is null)
        {
            return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
        }

        // Logica specifica per update command
        var existingDietRes = await _dietService.GetByIdAsync(id);
        if (existingDietRes.Data is null)
        {
            return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
        }

        // Preserva audit trail per update command
        var newDto = _createDtoToDietDtoMapper.Map(createDto);
        newDto.CreatedAt = existingDietRes.Data.CreatedAt; // Preserva created
        newDto.UpdatedAt = DateTime.UtcNow; // Aggiorna timestamp
        newDto.Id = id;

        return await _dietService.UpdateAsync(newDto);
    }
}
```

### Auth Command Handler

```csharp
// AuthManager gestisce command di autenticazione
internal class AuthManager : IAuthManager
{
    // Execute command: UserRegistrationRequest
    public async Task<BusinessResponse<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest request)
    {
        // Validazione command
        if (request is null)
        {
            return BusinessResponse<UserRegistrationResponse>.BadRequest(_responseMessageOptions.InvalidRequest);
        }

        // Controllo business rules
        var existingUser = await _authUserService.FindAsync(u => u.Email == request.Email);
        if (existingUser.Data?.ToList().Count != 0)
        {
            return BusinessResponse<UserRegistrationResponse>.BadRequest(_responseMessageOptions.UserAlreadyExists);
        }

        // Esecuzione command
        var userDto = _registrationRequestMapper.Map(request);
        userDto.Id = Guid.NewGuid();
        userDto.CreatedAt = DateTime.UtcNow;
        userDto.UpdatedAt = userDto.CreatedAt;

        var createResult = await _authUserService.CreateAsync(userDto);
        if (createResult.Data is null)
        {
            return BusinessResponse<UserRegistrationResponse>.InternalServerError(_responseMessageOptions.ErrorCreatingEntity);
        }

        // Mapping del risultato
        var response = _userResponseMapper.Map(createResult.Data);
        return BusinessResponse<UserRegistrationResponse>.Ok(response, _responseMessageOptions.UserCreatedSuccessfully);
    }

    // Execute command: UserLoginRequest
    public async Task<BusinessResponse<TokenResponse>> LoginUserAsync(UserLoginRequest request)
    {
        // Command handler per login
        // Logica di autenticazione e generazione token
    }
}
```

### Controller as Command Client

```csharp
// Controller agisce come Client che invia commands
[ApiController]
[Route("api/[controller]")]
public class DietController : ControllerBase
{
    private readonly IManager<DietDto, CreateDietRequest, int> _dietManager;

    // Client che utilizza Command Handler
    [HttpPost]
    public async Task<IActionResult> CreateDiet([FromBody] CreateDietRequest request)
    {
        // Invio del command al command handler
        var userIdClaim = User.FindFirst("userId");
        var result = await _dietManager.CreateAsync(request, userIdClaim);

        // Gestione del risultato del command
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiet(int id, [FromBody] CreateDietRequest request)
    {
        // Invio command di update
        var userIdClaim = User.FindFirst("userId");
        var result = await _dietManager.UpdateAsync(request, id, userIdClaim);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
```

### Command Validation Template

```csharp
// BaseManager fornisce template per validazione commands
internal abstract class BaseManager<TRequest> : IValidationManager<TRequest>
    where TRequest : class
{
    // Template method per validazione commands
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

    // Validazione specifica del command
    public TRequest? ValidateRequest(TRequest request)
    {
        if (request is null)
        {
            return null;
        }
        return request;
    }
}
```

## Vantaggi dell'Implementazione Attuale

1. **Incapsulamento**: Richieste incapsulate in oggetti command
2. **Disaccoppiamento**: Controller non conoscono implementazione handlers
3. **Riusabilità**: Command objects riutilizzabili in contesti diversi
4. **Testabilità**: Command handler testabili indipendentemente
5. **Audit**: Possibilità di logging e tracking dei command
6. **Validation**: Validazione centralizzata nei command handler

## Caratteristiche del Pattern nella Soluzione

1. **Command Objects**: Request DTOs che incapsulano parametri
2. **Command Handlers**: Manager che eseguono i command
3. **Invokers**: Controller che inviano command
4. **Receivers**: Services che eseguono operazioni concrete
5. **Command Interface**: Interfacce standard per command handler

## Vantaggi del Pattern

- **Disaccoppiamento**: Invoker separato da receiver
- **Extensibilità**: Facile aggiungere nuovi command
- **Undo Operations**: Supporto per operazioni di annullamento (implementabile)
- **Logging**: Possibilità di loggare tutti i command
- **Queuing**: Command possono essere accodati (implementabile)
- **Macro Commands**: Combinazione di command multipli (implementabile)

## Applicazione Corretta

Il pattern è implementato correttamente:

- Command objects ben definiti per ogni operazione
- Command handler dedicati per ogni dominio
- Interfacce standard per command execution
- Separazione chiara tra client e handler
- Validazione e business logic nei handler

## Possibili Estensioni

Il pattern attuale potrebbe essere esteso con:

```csharp
// Command con supporto per Undo
public interface IUndoableCommand<TRequest>
{
    Task<BusinessResponse<bool>> UndoAsync(TRequest request);
}

// Command Queue per processing asincrono
public interface ICommandQueue<TCommand>
{
    Task EnqueueAsync(TCommand command);
    Task<TCommand> DequeueAsync();
}

// Composite Command per operazioni multiple
public class CompositeCommand<TRequest>
{
    private readonly List<ICommand<TRequest>> _commands = new();

    public void Add(ICommand<TRequest> command) => _commands.Add(command);

    public async Task<BusinessResponse> ExecuteAllAsync(TRequest request)
    {
        foreach (var command in _commands)
        {
            var result = await command.ExecuteAsync(request);
            if (!result.IsSuccess) return result;
        }
        return BusinessResponse.Success();
    }
}
```

## Pattern Correlati

- Spesso utilizzato con **Observer Pattern** per notificare l'esecuzione
- Si integra con **Strategy Pattern** per diverse modalità di esecuzione
