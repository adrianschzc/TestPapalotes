# 🎓 Sistema de Control Escolar

Aplicación web para el control escolar de una universidad, desarrollada como examen práctico.

- **Framework:** ASP.NET Core MVC (.NET 10)
- **Lenguaje:** C#
- **Base de datos:** Microsoft SQL Server (LocalDB) con **Entity Framework Core**
- **Consultas:** LINQ

---

## 📋 Requerimientos cubiertos

| # | Requerimiento | Dónde está | Valor |
|---|---------------|-----------|-------|
| Admin 1 | Asignación de **roles** a usuarios registrados por grupo | `Usuarios` (Administración → Roles de usuarios) | 20% |
| Admin 2 | **Mantenimiento** (CRUD) de materias, grupos, profesores y alumnos | `Materias`, `Grupos`, `Profesores`, `Alumnos` | 15% |
| Admin 3 | **Asignar** profesores y alumnos a las materias | `Asignaciones` | 15% |
| Docente 1 | Registrar **calificaciones** por número de boleta | `Docente` (Panel Docente) | 30% |
| Alumno 1 | **Consultar** calificaciones por número de boleta | `AlumnoPanel` (Panel Alumno) | 20% |

---

## 🧱 Modelo de datos

```
Grupo 1───* Alumno
Grupo 1───* Asignacion *───1 Materia
                │
                *───1 Profesor
Alumno 1───* Calificacion *───1 Asignacion
Usuario *───1 Grupo   (con Rol: Administrador | Profesor | Alumno)
```

Una **Asignación** vincula una *materia* con un *grupo* y el *profesor* que la imparte.
Los alumnos de ese grupo quedan inscritos automáticamente en la materia, y sus
**calificaciones** se registran contra esa asignación.

---

## ✅ Requisitos previos

- **Visual Studio 2022/2026** (o VS Code) con la carga de trabajo *ASP.NET y desarrollo web*.
- **.NET 10 SDK** — verifícalo con `dotnet --version`.
- **SQL Server LocalDB** (viene incluido con Visual Studio). Verifícalo con:
  ```powershell
  sqllocaldb info
  ```
  Si no existe la instancia por defecto, créala con `sqllocaldb create MSSQLLocalDB`.

---

## ▶️ Cómo levantar el proyecto

### Opción A — Visual Studio
1. Abre `TestPapalotes.slnx`.
2. Presiona **F5** (o el botón ▶ *TestPapalotes*).
3. El navegador abre automáticamente en `https://localhost:xxxx`.

### Opción B — Línea de comandos
```powershell
cd TestPapalotes\TestPapalotes
dotnet run
```
Abre la URL que aparece en la consola (por ejemplo `http://localhost:5201`).

> **La base de datos se crea sola.** Al arrancar, `DbSeeder` ejecuta
> `EnsureCreated()` y carga datos de ejemplo automáticamente — no hay que correr
> migraciones ni scripts SQL a mano.

---

## 🗄️ Base de datos

- **Cadena de conexión** (en `appsettings.json`):
  ```
  Server=(localdb)\MSSQLLocalDB;Database=ControlEscolar;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
  ```
- Se crea una base llamada **`ControlEscolar`** en tu LocalDB.
- Para inspeccionarla: Visual Studio → *Ver → Explorador de objetos de SQL Server* →
  `(localdb)\MSSQLLocalDB` → Bases de datos → `ControlEscolar`.
- Para **reiniciar los datos**: borra la base `ControlEscolar` (clic derecho → Eliminar)
  y vuelve a ejecutar; se recreará y sembrará de nuevo.

### Datos de prueba precargados
| Grupo | Alumnos (boleta) | Materias | Profesor |
|-------|------------------|----------|----------|
| `3CV1` | Ana García `2023630001`, Luis Ramírez `2023630002` | Matemáticas I, Programación | Juan Pérez, Ana López |

Usuarios: `admin` (Administrador), `jperez` (Profesor), `agarcia` (Alumno).

---

## 🧪 Cómo probar cada módulo

1. **Roles** → menú *Administración → Roles de usuarios*: crea/edita usuarios y asígnales rol y grupo.
2. **Catálogos** → *Administración → Materias / Grupos / Profesores / Alumnos*: alta, edición y baja.
3. **Asignaciones** → *Administración → Asignaciones*: liga materia + grupo + profesor. Botón *Alumnos* muestra los inscritos.
4. **Panel Docente** → escribe la boleta `2023630001`, captura calificaciones y guarda.
5. **Panel Alumno** → escribe la boleta `2023630001` y verás las calificaciones capturadas (verde ≥ 60, rojo < 60).

---

## 📁 Estructura del proyecto

```
TestPapalotes/
├── Controllers/     # Materias, Grupos, Profesores, Alumnos, Asignaciones,
│                    # Usuarios, Docente, AlumnoPanel, Home
├── Models/          # Materia, Grupo, Profesor, Alumno, Asignacion,
│                    # Calificacion, Usuario, ViewModels
├── Data/
│   ├── ControlEscolarContext.cs   # DbContext de EF Core
│   └── DbSeeder.cs                # Crea la BD y siembra datos
├── Views/           # Vistas Razor por controlador
├── appsettings.json # Cadena de conexión
└── Program.cs       # Configuración + registro del DbContext + seed
```

---

## 📦 Cómo entregar (comprimir la solución)

Antes de comprimir, borra las carpetas generadas para reducir el tamaño:

```powershell
Remove-Item -Recurse -Force TestPapalotes\bin, TestPapalotes\obj, .vs -ErrorAction SilentlyContinue
Compress-Archive -Path * -DestinationPath ControlEscolar.zip
```

Sube `ControlEscolar.zip` a tu repositorio o liga de descarga. Al descomprimir,
basta con abrir `TestPapalotes.slnx` y presionar **F5**.
