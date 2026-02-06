# Configuratio  à la base de données
Il faut changer l'URL de la base de donnée ! 
Url à initliser dans : `ConnectionStrings` de `appsettings.json`

# Build :
```bash
dotnet clean    
dotnet restore
dotnet build
```

## Migration
```bash
dotnet ef migrations add [Nom d'une migration]
dotnet ef database update
```

# Lancer le programme (en https) :
```bash
dotnet run --launch-profile https
```
L'API devrait être accessible sur :
- **HTTPS** : `https://localhost:7073`
- **HTTP** : `http://localhost:5073`

# Swagger
Aller sur l'url : `https://localhost:7073/swagger`
