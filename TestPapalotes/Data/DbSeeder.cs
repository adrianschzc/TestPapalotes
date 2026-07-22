using Microsoft.EntityFrameworkCore;
using TestPapalotes.Models;

namespace TestPapalotes.Data
{
    // Crea la base de datos (si no existe) y carga datos de ejemplo
    // para poder probar todos los módulos sin capturar nada a mano.
    public static class DbSeeder
    {
        public static void Seed(ControlEscolarContext db)
        {
            // Aplica las migraciones pendientes: crea las tablas dentro de la
            // base de datos (que en hosting ya existe, aunque esté vacía).
            db.Database.Migrate();

            if (db.Materias.Any()) return; // Ya sembrado.

            var grupo = new Grupo { Clave = "3CV1", Nombre = "3er Semestre - Vespertino" };
            var grupo2 = new Grupo { Clave = "1MM2", Nombre = "1er Semestre - Matutino" };
            db.Grupos.AddRange(grupo, grupo2);

            var mate = new Materia { Clave = "MAT101", Nombre = "Matemáticas I", Creditos = 8 };
            var prog = new Materia { Clave = "PRO201", Nombre = "Programación", Creditos = 10 };
            db.Materias.AddRange(mate, prog);

            var profJuan = new Profesor { NumEmpleado = "E-100", Nombre = "Juan Pérez", Correo = "juan@uni.mx" };
            var profAna = new Profesor { NumEmpleado = "E-200", Nombre = "Ana López", Correo = "ana@uni.mx" };
            db.Profesores.AddRange(profJuan, profAna);

            var ana = new Alumno { Boleta = "2023630001", Nombre = "Ana García", Correo = "ana.g@uni.mx", Grupo = grupo };
            var luis = new Alumno { Boleta = "2023630002", Nombre = "Luis Ramírez", Correo = "luis.r@uni.mx", Grupo = grupo };
            db.Alumnos.AddRange(ana, luis);

            // Materias impartidas en el grupo por un profesor.
            var asigMate = new Asignacion { Materia = mate, Grupo = grupo, Profesor = profJuan };
            var asigProg = new Asignacion { Materia = prog, Grupo = grupo, Profesor = profAna };
            db.Asignaciones.AddRange(asigMate, asigProg);

            // Inscripciones de ejemplo: ambos alumnos quedan asignados a las dos materias.
            db.Inscripciones.AddRange(
                new Inscripcion { Alumno = ana, Asignacion = asigMate },
                new Inscripcion { Alumno = ana, Asignacion = asigProg },
                new Inscripcion { Alumno = luis, Asignacion = asigMate },
                new Inscripcion { Alumno = luis, Asignacion = asigProg }
            );

            db.Usuarios.AddRange(
                new Usuario { NombreUsuario = "admin", NombreCompleto = "Administrador del Sistema", Rol = Rol.Administrador },
                new Usuario { NombreUsuario = "jperez", NombreCompleto = "Juan Pérez", Rol = Rol.Profesor },
                new Usuario { NombreUsuario = "agarcia", NombreCompleto = "Ana García", Rol = Rol.Alumno, Grupo = grupo }
            );

            db.SaveChanges();
        }
    }
}
