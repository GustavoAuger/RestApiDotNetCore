# SupabaseApiDemo

Este proyecto es una **API RESTful en .NET Core** que gestiona usuarios, utilizando **Entity Framework Core** como ORM y **PostgreSQL** como base de datos. Está preparada para crecer, incluyendo validaciones, hasheo de contraseñas y buenas prácticas de desarrollo.

## 📦 Estructura del Proyecto

│
├── Controllers/ # Controladores de la API
├── Models/ # Entidades del dominio (por ahora solo Usuario)
├── Data/ # DbContext y configuración de EF Core
├── Services/ # Lógica de negocio (UsuarioService)
├── Utils/ # Utilidades: validaciones y hash de contraseñas
└── Program.cs # Configuración principal de la aplicación

---

## 🔧 Tecnologías Usadas

- **.NET 9**
- **Entity Framework Core 9**
- **PostgreSQL**
- **BCrypt.Net** – Hasheo seguro de contraseñas
- **DotNetEnv** – Carga de variables desde `.env`

---

## 🔐 Variables de Entorno

El proyecto utiliza un archivo `.env` para gestionar la configuración sensible de la conexión a la base de datos:

```env
DB_HOST=localhost
DB_PORT=5432
DB_USER=tu_usuario
DB_PASSWORD=tu_contraseña
DB_NAME=nombre_de_base
```
--- 

## ⚙️ Funcionalidades principales

### UsuarioService

Contiene la lógica para operar con usuarios activos:

- `GetAllUsuariosAsync()` → Obtener todos los usuarios activos.
- `GetUsuarioByIdAsync(int id)` → Obtener usuario por ID.
- `GetUsuarioByEmailAsync(string email)` → Obtener usuario por email.
- `CreateUsuarioAsync(Usuario usuario)` → Crear un nuevo usuario con validaciones.
- `UpdateUsuarioAsync(int id, Usuario usuario)` → Actualizar datos de usuario (email, nombre, rol, bodega, contraseña).
- `DeleteUsuarioAsync(int id)` → Eliminación lógica (soft delete) del usuario (`Estado = false`).

### Utilidades incluidas

- **Validaciones**:
  - Validación si el correo ya existe y está activo.
  - Validación por ID de usuario activo.
- **Hasheo de contraseñas**:
- Utiliza funciones de hash seguras mediante `BCrypt` para almacenar contraseñas de forma segura, tal como se implementa en `PassworHash`.


---

## 🔒 Seguridad

- De momento **no se ha implementado JWT** para autenticación, pero el proyecto está preparado para ello como siguiente paso.
- Todas las contraseñas se almacenan de forma segura con hash.

---

## 🧪 Ejemplo de uso de la API

### Crear un nuevo usuario

`POST /api/usuarios`

```json
{
  "email": "ejemplo@correo.com",
  "nombre": "ejemplo",
  "contrasena": "MiClave123!",
  "idRol": 1,
  "idBodega": 2
}
```
### Actualizar un usuario

`PUT /api/usuarios/id`

```json
{
  "email": "nuevo@email.com",
  "nombre": "Nuevo Nombre",
  "idRol": 2,
  "idBodega": 3,
  "contrasena": "nuevaPassword123"
}
```
###  Eliminar (soft delete)
`DELETE /api/usuarios/id`

###  Consulta usuarios por id
`GET /api/usuarios/id`

###  Consulta usuarios por email
`GET /api/usuarios/email/@email`

###  Consulta todos los usuarios
`GET /api/usuarios/`

