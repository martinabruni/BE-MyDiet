# BE-MyDiet

A Domain-Driven Design (DDD) skeleton template for .NET backend projects.  
This template sets up a clean architecture with layers for Domain, Business, Infrastructure and API, ready to plug in your entities, value objects, aggregates and application services.

---

## Prerequisites

- [.NET 8.0 SDK (or later)](https://dotnet.microsoft.com/download)  
- `git` (to clone this repo, if installing locally)

---

## Installing the template

### From this repository (local)

```bash
# 1. Clone the repo
git clone https://github.com/<your-org>/BE-MyDiet.git
cd BE-MyDiet

# 2. Install the template from the local folder
dotnet new install .
````

### Directly from GitHub

```bash
dotnet new install https://github.com/<your-org>/BE-MyDiet
```

> **Note:** Replace `<your-org>` with your GitHub organization or username.

---

## Creating a new project

Once the template is installed, scaffold your DDD-based project with:

```bash
dotnet new BE-MyDiet \
  --name MyAwesomeService \
  --output MyAwesomeService
```

This will produce a folder structure like:

```
MyAwesomeService/
├───src
│   ├───MyAwesomeService.Apis
|       ├───MyAwesomeService.Core.Api
│   ├───MyAwesomeService.Businesses
|       ├───MyAwesomeService.Core.Business
│   ├───MyAwesomeService.Domains
|       ├───MyAwesomeService.Core.Domain
│   └───MyAwesomeService.Infrastructures
|       ├───MyAwesomeService.Core.Infrastructure
```

You can then:

```bash
cd MyAwesomeService
dotnet restore
dotnet build
```

---

## Uninstalling the template

```bash
# If installed from a folder
dotnet new uninstall .

# If installed from GitHub
dotnet new uninstall https://github.com/<your-org>/BE-MyDiet
```

---

## Additional help

```bash
# View all available template parameters
dotnet new BE-MyDiet --help
```

Feel free to customize the template parameters or folder layout as needed!
```
