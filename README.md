# To-do-list - Prueba técnica

Repositorio con el backend en **.NET** (`To-do-list`) y el frontend en **Angular** (`To-do-list-client`).

## Requisitos previos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) y npm
- [Docker](https://www.docker.com/) + Docker Compose
- [Angular CLI](https://angular.dev/tools/cli) (opcional, el proyecto ya lo usa via npm)

---

## 1. Base de datos (PostgreSQL con Docker)

Levanta un Postgres local con las credenciales que usa el backend en
`To-do-list/appsettings.Development.json`:

> `Host=localhost:5432;Database=tododb;Username=todouser;Password=123`

```bash
docker run --name tododb -p 5432:5432 \
  -e POSTGRES_DB=tododb \
  -e POSTGRES_USER=todouser \
  -e POSTGRES_PASSWORD=123 \
  -d postgres:16
```

El backend aplica las migraciones automáticamente al arrancar (`db.Database.Migrate()`),
así que no hace falta migrar a mano.

---

## 2. Backend (.NET / To-do-list)

```bash
cd To-do-list
dotnet restore
dotnet run
```

Se levanta en modo `Development` en `http://localhost:5239`.

- Documentación Swagger: `http://localhost:5239/swagger`
- Respeta la cadena de conexión de `appsettings.Development.json`.

---

## 3. Frontend (Angular `To-do-list-client`)

```bash
cd To-do-list-client
npm install
npm start
```

Se sirve en `http://localhost:4200`.

> La URL de la API está en `src/environments/environment.development.ts`
> (`API_BASE_URL`). Verifica que apunte al puerto donde corre el backend
> (por defecto el backend usa el `5239`, aunque en la configuración actual del
> cliente figura `http:localhost:3000/api/task`; ajústala al puerto real del backend).

---

## Notas

- Para detener el contenedor de Postgres: `docker rm -f tododb`.
- En producción (`Production`) el backend usa una base de datos remota definida en
  `appsettings.Production.json`.