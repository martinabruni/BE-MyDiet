# Design Patterns nella Soluzione MyDiet

Questa cartella contiene la documentazione di tutti i design patterns identificati e implementati nella soluzione MyDiet. Ogni pattern è documentato con esempi concreti tratti dal codice del progetto.

## Patterns Identificati

### Creational Patterns

#### 1. [Singleton Pattern](./Singleton_Pattern.md)

**File implementazione**: Configuration options, Message options, Azure services

- Garantisce una sola istanza di configurazioni e servizi costosi
- Implementato tramite Dependency Injection con lifetime Singleton
- Utilizzato per: TokenOption, KeyOption, SecretClient, MessageOptions

### Structural Patterns

#### 2. [Adapter Pattern](./Adapter_Pattern.md)

**File implementazione**: Authentication handlers, Database contexts, Extension methods

- Adatta interfacce incompatibili per lavorare insieme
- Integra componenti custom con framework esistenti (ASP.NET Core)
- Utilizzato per: CustomJwtAuthenticationHandler, MyDietAuthDb, ServiceCollection extensions

#### 3. [Decorator Pattern](./Decorator_Pattern.md)

**File implementazione**: Services, Managers, Repositories

- Aggiunge funzionalità a oggetti esistenti senza modificarne la struttura
- Implementato attraverso ereditarietà da classi base generiche
- Utilizzato per: AuthUserService, DietManager, PlanManager, Repository implementations

#### 4. [Facade Pattern](./Facade_Pattern.md)

**File implementazione**: Managers, Configuration extensions

- Fornisce interfacce semplificate per sottosistemi complessi
- Nasconde la complessità di interazioni multiple
- Utilizzato per: AuthManager, DietManager, PlanManager, ServiceCollection extensions

### Behavioral Patterns

#### 5. [Command Pattern](./Command_Pattern.md)

**File implementazione**: Request DTOs, Managers, Controllers

- Incapsula richieste come oggetti per disaccoppiare client e handler
- Permette parametrizzazione e gestione di operazioni
- Utilizzato per: CreateDietRequest, UserRegistrationRequest, Manager operations

#### 6. [Observer Pattern](./Observer_Pattern.md)

**File implementazione**: Middleware, Authentication, Application lifecycle

- Definisce dipendenze uno-a-molti per notifiche automatiche
- Implementato attraverso pipeline di middleware e eventi del framework
- Utilizzato per: CoreUserMiddleware, Authentication events, Application initialization

#### 7. [Strategy Pattern](./Strategy_Pattern.md)

**File implementazione**: Mappers, Services

- Definisce famiglia di algoritmi intercambiabili
- Implementato attraverso interfacce comuni e implementazioni specifiche
- Utilizzato per: Mapping strategies, Service implementations

#### 8. [Template Method Pattern](./Template_Method_Pattern.md)

**File implementazione**: Base classes, Manager inheritance

- Definisce scheletro di algoritmi con passi personalizzabili
- Implementato attraverso classi base astratte
- Utilizzato per: BaseManager, AGenericService, AGenericRepository

### Architectural Patterns

#### 9. [Repository Pattern](./Repository_Pattern.md)

**File implementazione**: Repository interfaces e implementations

- Fornisce astrazione per l'accesso ai dati
- Disaccoppia business logic dal data access layer
- Utilizzato per: AuthUserRepository, DietRepository, PlanRepository, CoreUserRepository

#### 10. [Service Layer Pattern](./Service_Layer_Pattern.md)

**File implementazione**: Service interfaces e implementations

- Definisce confine applicativo con operazioni business
- Coordina risposte e incapsula logica di business
- Utilizzato per: AuthUserService, DietService, PlanService, CoreUserService

#### 11. [Dependency Injection Pattern](./Dependency_Injection_Pattern.md)

**File implementazione**: Extension methods, Program.cs, Constructor injection

- Implementa Inversion of Control per gestione dipendenze
- Promuove disaccoppiamento e testabilità
- Utilizzato in: Tutta la soluzione per configurazione e injection

## Architettura Generale

La soluzione implementa un'architettura layered pulita con i seguenti livelli:

```
Controllers (API Layer)
    ↓ (Command Pattern)
Managers (Business Facade)
    ↓ (Service Layer + Template Method)
Services (Business Logic)
    ↓ (Repository Pattern)
Repositories (Data Access)
    ↓ (Adapter Pattern)
DbContext (Entity Framework)
```

## Pattern Relationships

I patterns nella soluzione lavorano insieme sinergicamente:

- **Template Method** + **Decorator**: Base classes con decorazioni specifiche
- **Command** + **Facade**: Commands processati da facade managers
- **Strategy** + **Dependency Injection**: Strategies configurate tramite DI
- **Observer** + **Adapter**: Middleware observers che adattano HTTP context
- **Repository** + **Service Layer**: Repository astratti usati dai services
- **Singleton** + **Dependency Injection**: Singletons gestiti dal DI container

## Benefici dell'Implementazione

1. **Manutenibilità**: Codice organizzato e facilmente modificabile
2. **Testabilità**: Dipendenze mockabili e logica isolata
3. **Estensibilità**: Facile aggiungere nuove funzionalità
4. **Riusabilità**: Componenti riutilizzabili tra domini
5. **Separazione delle responsabilità**: Ogni layer ha responsabilità specifiche
6. **Disaccoppiamento**: Bassa dipendenza tra componenti

## Conformità ai Principi SOLID

- **Single Responsibility**: Ogni classe ha una responsabilità specifica
- **Open/Closed**: Estensibili senza modifiche (Decorator, Strategy)
- **Liskov Substitution**: Implementazioni sostituibili (Repository, Service)
- **Interface Segregation**: Interfacce specifiche e focalizzate
- **Dependency Inversion**: Dipendenze da astrazioni, non da concrezioni

## Note sull'Implementazione

Tutti i patterns sono implementati correttamente seguendo le best practices e integrandosi naturalmente con il framework ASP.NET Core e Entity Framework. L'uso di generics e interfacce riduce la duplicazione del codice mantenendo type safety e performance.

La documentazione di ciascun pattern include:

- Descrizione teorica del pattern
- File specifici coinvolti nell'implementazione
- Esempi di codice concreti
- Vantaggi dell'implementazione attuale
- Relazioni con altri patterns
- Possibili miglioramenti o estensioni
