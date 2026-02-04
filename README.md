# Configuratio  à la base de données
Il faut changer l'URL de la abse de donnée si besoin (à voir)

# Build :
```bash
dotnet clean    
dotnet restore
dotnet build
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
