# Installation de packages nécéssaires
```bash
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

# Configuratio  à la base de données
Il faut changer l'URL de la base de donnée ! 
Url à initliser dans : `ConnectionStrings` de `appsettings.json`

# Build :
```bash
dotnet clean    
dotnet restore
dotnet build
```

# Migration
```bash
Add-Migration [Nom d'une migration]
Update-Database
```

# Lancer le programme (en https) :
```bash
dotnet run --launch-profile https
```
L'API devrait être accessible sur :
- **HTTPS** : `https://localhost:7073` ([voir swagger](https://localhost:7073/swagger))
- **HTTP** : `http://localhost:5073`([voir swagger](https://localhost:5073/swagger))

---
# Sources 
## EF
- [Création d'entities pour code first](https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=fluent-api%2Cwith-nrt)
- [Orgnaisation avecIEntityTypeConfiguration](https://medium.com/munchy-bytes/organizing-entity-configurations-with-ientitytypeconfiguration-in-entity-framework-core-f5a2e290ec04)
- [Migration](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs)
