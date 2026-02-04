# Demo 1 – API .NET 8 Contenerizada con Docker (sin base de datos)

## 🎯 Objetivo del demo

Este demo demuestra los **fundamentos de la contenerización** usando Docker con una **ASP.NET Core Web API (.NET 8)**, enfocándose en:

- Construcción de imágenes reproducibles
- Separación entre código y configuración
- Uso de variables de entorno (12-Factor App)
- Ejecución consistente en cualquier entorno

No incluye base de datos ni orquestación avanzada.  
Sirve como **base conceptual** para los siguientes demos.

---

## 🧱 Arquitectura del demo

- **Aplicación:** ASP.NET Core Web API (.NET 8 – Minimal API)
- **Contenerización:** Docker (Dockerfile multi-stage)
- **Configuración:** Variables de entorno
- **Exposición:** HTTP (puerto 8080)
- **Persistencia:** No aplica (API stateless)

---

## 🚀 Endpoints disponibles

| Endpoint | Método | Descripción |
|--------|--------|-------------|
| `/health` | GET | Verificación básica de salud |
| `/info` | GET | Información de la aplicación y entorno |
| `/greet?name=Ricardo` | GET | Saludo configurable |
| `/swagger` | GET | Documentación OpenAPI |

---

## 🐳 Construcción de la imagen Docker

Desde la carpeta `demo01-api-only/`:

```bash
docker build -t demo01-api:1.0 .

docker run --rm --name demo01-api \
  -p 8080:8080 \
  -e APP_NAME="Demo01.Api" \
  -e APP_VERSION="1.0.0" \
  -e GREETING_PREFIX="Hola desde Docker" \
  -e ASPNETCORE_ENVIRONMENT="Production" \
  demo01-api:1.0

 ```

---

## 👤 Autor
**Ricardo Jara Gaspar**  
Ingeniero de Software especializado en .NET y Arquitectura de Software  
🔗 [GitHub](https://github.com/RJARAG-92) · [LinkedIn](https://www.linkedin.com/in/ricardojarag) · 🇵🇪 Perú

