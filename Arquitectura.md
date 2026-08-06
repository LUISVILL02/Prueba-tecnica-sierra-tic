# Arquitectura y patrones utilizados

## Backend (`To-do-list` — ASP.NET Core + EF Core + PostgreSQL)

Use una arquitectura en capas con una separación clara `Controllers → Services → Repositories → Data/Entities`.

- Controller → Services → Repositories → Data.
- El *controller* solo orquesta y delega en los servicios.
- El *service* aplica la lógica de negocio y convierte DTOs ↔ entidades.
- El *repository* encapsula el acceso a datos (EF Core).

Usé el patron repositorio para que el servicio no dependa de la infraestructura de datos.

Tambien la logica de negoco se aplico en la capa de servicio.

Por otro lado, se uso el patron de inyección de dependencias para aplicar uno de los principio SOLID (inversión de control) registrando las interfacesen el scope con sus respectivas implementaciones.

Como bien indicaba la prueba, la solucion fue basada en una API rest que consumiria un cliente.


## Frontend (`frontend` — Angular + Signals + nuevo control de flujo)

Aca se uso la arquitectura basada em features y nuevamente se uso el patron de inyección de dependencias.

