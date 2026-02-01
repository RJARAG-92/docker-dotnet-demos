# Demo 2 – API .NET 8 + PostgreSQL con Docker Compose (sin EF Core)

## 🎯 Objetivo

Este demo demuestra cómo construir y ejecutar un **sistema mínimo compuesto por múltiples servicios**
utilizando **Docker Compose**, integrando:

- Una **API .NET 8** (stateless)
- Una **base de datos PostgreSQL** (stateful)
- Persistencia de datos mediante **volúmenes Docker**
- Configuración externa por **variables de entorno**
- Separación de responsabilidades usando un **patrón repositorio liviano** (sin EF Core)

Caso de uso: **Task Manager** (CRUD básico).

---

## 🧱 Arquitectura

- **API:** ASP.NET Core Web API (.NET 8 – Minimal API)
- **DB:** PostgreSQL 16
- **Orquestación local:** Docker Compose
- **Persistencia:** Named volume (`demo02_pgdata`)
- **Configuración:** Variables de entorno (`ConnectionStrings__Default`)
- **Acceso a datos:** Npgsql + Repository (thin)

---

## 📂 Estructura del proyecto

```
demo02-api-db/
 ├─ src/
 │   └─ Demo02.Api/
 │       ├─ Program.cs
 │       ├─ appsettings.json
 │       ├─ Models/
 │       ├─ Contracts/
 │       └─ Data/
 ├─ Dockerfile
 ├─ docker-compose.yml
 ├─ .dockerignore
 └─ README.md
```

---

## 🚀 Levantar el sistema

Desde la carpeta `demo02-api-db/`:

```bash
docker compose up --build
```

Servicios levantados:

- **API:** http://localhost:8081
- **Swagger (OpenAPI):** http://localhost:8081/swagger
- **PostgreSQL (host):**
  - Host: `localhost`
  - Puerto: `5433`
  - DB: `demo02`
  - Usuario: `postgres`
  - Password: `postgres`

---

## 📘 Evidencia de Swagger (OpenAPI)

Una vez levantado el sistema con Docker Compose, la API expone su contrato mediante **Swagger UI**.

👉 **Swagger UI:**  
http://localhost:8081/swagger

Desde Swagger es posible:

- Visualizar todos los endpoints disponibles
- Inspeccionar los modelos (`TaskItem`, `CreateTaskRequest`)
- Ejecutar peticiones `GET`, `POST`, `PUT` y `DELETE`
- Validar códigos de respuesta HTTP (`200`, `201`, `400`, `404`)
- Confirmar la correcta integración entre la API y PostgreSQL

Swagger actúa como **evidencia funcional** de que:
- la API se ejecuta dentro de Docker
- los endpoints están correctamente mapeados
- la persistencia en base de datos funciona correctamente

---

## ✅ Endpoints disponibles

| Endpoint | Método | Descripción |
|--------|--------|-------------|
| `/health` | GET | Verificación básica de salud |
| `/tasks` | GET | Listar tareas |
| `/tasks` | POST | Crear tarea |
| `/tasks/{id}` | GET | Obtener tarea por id |
| `/tasks/{id}/done` | PUT | Marcar tarea como realizada |
| `/tasks/{id}` | DELETE | Eliminar tarea |

---

## 🧪 Pruebas rápidas (curl)

### Crear tarea
```bash
curl -X POST http://localhost:8081/tasks \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Aprender Docker Compose\"}"
```

### Listar tareas
```bash
curl http://localhost:8081/tasks
```

### Marcar tarea como realizada
```bash
curl -X PUT http://localhost:8081/tasks/<GUID>/done
```

### Eliminar tarea
```bash
curl -X DELETE http://localhost:8081/tasks/<GUID>
```

---

## 💾 Persistencia de datos

Los datos de PostgreSQL se almacenan en un **named volume**.

- Detener servicios:
```bash
docker compose down
```

- Detener y borrar datos (reset DB):
```bash
docker compose down -v
```

---

## 🧠 Conceptos técnicos demostrados

- Docker Compose para aplicaciones multi-servicio
- Comunicación entre contenedores por nombre de servicio (`Host=db`)
- Persistencia con volúmenes Docker
- Configuración externa (12-Factor App)
- API stateless + DB stateful
- Patrón repositorio sin sobreingeniería (sin EF Core)
- Bootstrap automático de la base de datos
- Documentación y prueba de API con Swagger (OpenAPI)

---

## 📌 Notas finales

Este demo prioriza **claridad arquitectónica y foco en infraestructura**, dejando
frameworks más pesados (como EF Core) para escenarios más complejos.

Es la **base natural** para el siguiente demo orientado a arquitectura enterprise.
