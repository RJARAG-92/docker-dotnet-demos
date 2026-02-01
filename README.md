# Docker & .NET Architecture Demos

Este repositorio contiene una serie de **demos progresivos** orientados a demostrar el uso de **Docker**, **Docker Compose** y **.NET 8**, aplicando buenas prácticas de **arquitectura de software**, configuración externa y diseño limpio.

El objetivo no es solo “hacer que funcione”, sino **mostrar criterio técnico y evolución arquitectónica**, desde un contenedor simple hasta sistemas más completos.

---

## 🎯 Objetivos del repositorio

- Comprender y aplicar **contenedorización con Docker**
- Construir aplicaciones **portables y reproducibles**
- Separar responsabilidades entre **aplicación e infraestructura**
- Aplicar principios de la **12-Factor App**
- Demostrar progresión técnica hacia un **perfil de Arquitecto de Software**

---

## 🧩 Demos incluidas (avance actual)

### ✅ Demo 1 – API .NET 8 contenida con Docker
📁 `demo01-api-only/`

**Qué demuestra:**
- API mínima en .NET 8
- Dockerfile multi-stage
- Contenedor autoejecutable
- Configuración por variables de entorno
- Eliminación de dependencias del IDE (`launchSettings.json`)

**Conceptos clave:**
- Imagen vs contenedor
- Build y run con Docker
- Aplicación stateless
- Base para despliegue cloud

👉 Ideal como primer paso para entender Docker aplicado a .NET.

---

### ✅ Demo 2 – API .NET 8 + PostgreSQL con Docker Compose
📁 `demo02-api-db/`

**Qué demuestra:**
- Aplicación **multi-servicio**
- Orquestación local con Docker Compose
- Comunicación entre contenedores
- Persistencia de datos con volúmenes
- Configuración externa (12-Factor)
- Patrón repositorio liviano (sin EF Core)

**Arquitectura:**
- API .NET 8 (stateless)
- PostgreSQL 16 (stateful)
- Named volumes para persistencia
- Bootstrap automático de base de datos
- Documentación y prueba con Swagger (OpenAPI)

**Caso de uso:**
- Task Manager (CRUD básico)

👉 Este demo consolida el uso real de Docker en un escenario cercano a producción.

---

## 🏗️ Estructura del repositorio

```
docker-dotnet-demos/
├─ demo01-api-only/
│ └─ README.md
├─ demo02-api-db/
│ └─ README.md
└─ README.md ← (este archivo)
```


Cada demo es **independiente**, ejecutable y documentado.

---

## 🧠 Principios y buenas prácticas aplicadas

- **12-Factor App**
  - Configuración por variables de entorno
  - Servicios externos desacoplados
- **Stateless services**
- **Separación de responsabilidades**
- **Infraestructura como código (local)**
- **Evitar sobreingeniería**
- **Elección consciente de herramientas**
  - EF Core se reserva para demos más avanzados

---

## 🚀 Cómo usar este repositorio

1. Clonar el repositorio
2. Entrar al demo deseado
3. Seguir el `README.md` del demo
4. Ejecutar con Docker / Docker Compose

Ejemplo:
```bash
cd demo02-api-db
docker compose up --build
