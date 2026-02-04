# Docker & .NET Architecture Demos

Este repositorio contiene una serie de **demos progresivos** orientados a demostrar el uso de **Docker**, **Docker Compose** y **.NET 8**, aplicando buenas prácticas de **arquitectura de software**, configuración externa y diseño limpio.

El objetivo no es solo “hacer que funcione”, sino **mostrar criterio técnico y evolución arquitectónica**, desde un contenedor simple hasta una **plataforma distribuida basada en DDD**.

---

## 🎯 Objetivos del repositorio

- Comprender y aplicar **contenedorización con Docker**
- Construir aplicaciones **portables y reproducibles**
- Orquestar múltiples servicios con **Docker Compose**
- Separar responsabilidades entre **aplicación, dominio e infraestructura**
- Aplicar principios de la **12-Factor App**
- Introducir **DDD, CQRS y arquitectura orientada a eventos**
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

### ✅ Demo 3 – Plataforma distribuida con DDD, eventos y Worker
📁 `demo03-platform/`

**Qué demuestra:**
- **Domain-Driven Design (DDD)** con dominio como núcleo
- **Clean Architecture** con dependencias hacia adentro
- **CQRS** (Commands & Queries)
- **Arquitectura orientada a eventos**
- **Worker asíncrono** desacoplado de la API
- **Auditoría de negocio (Audit Trail)**
- **Cache distribuido con Redis**
- **Mensajería con RabbitMQ**
- **Migraciones automáticas con EF Core**
- **Despliegue completo con Docker Compose**

**Arquitectura:**
- API HTTP (.NET 8)
- Worker independiente (.NET 8)
- PostgreSQL (persistencia)
- Redis (cache)
- RabbitMQ (mensajería)
- MailHog (SMTP de pruebas)

**Conceptos clave:**
- Dominio independiente de frameworks
- Casos de uso como orquestadores
- Side-effects procesados fuera del request HTTP
- Idempotencia en procesamiento asíncrono
- Infraestructura declarativa y reproducible

👉 Este demo representa un **salto arquitectónico**, mostrando cómo diseñar, desplegar y operar una solución distribuida real.

---

## 🏗️ Estructura del repositorio

```
docker-dotnet-demos/
├─ demo01-api-only/
│ └─ README.md
├─ demo02-api-db/
│ └─ README.md
├─ demo03-platform/
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
- **Infraestructura como código (local)**
- **Separación clara de responsabilidades**
- **DDD (Domain-Driven Design)**
  - Dominio como núcleo
  - Infraestructura como detalle
- **Arquitectura orientada a eventos**
- **Evolución progresiva, sin sobreingeniería**
- **Elección consciente de herramientas**
  - EF Core introducido solo cuando el dominio lo justifica

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
```

---

## 👤 Autor
**Ricardo Jara Gaspar**  
Ingeniero de Software especializado en .NET y Arquitectura de Software  
🔗 [GitHub](https://github.com/RJARAG-92) · [LinkedIn](https://www.linkedin.com/in/ricardojarag) · 🇵🇪 Perú

