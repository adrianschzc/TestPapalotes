using Microsoft.EntityFrameworkCore;
using TestPapalotes.Models;

namespace TestPapalotes.Data
{
    public class ControlEscolarContext : DbContext
    {
        public ControlEscolarContext(DbContextOptions<ControlEscolarContext> options)
            : base(options) { }

        public DbSet<Materia> Materias => Set<Materia>();
        public DbSet<Grupo> Grupos => Set<Grupo>();
        public DbSet<Profesor> Profesores => Set<Profesor>();
        public DbSet<Alumno> Alumnos => Set<Alumno>();
        public DbSet<Asignacion> Asignaciones => Set<Asignacion>();
        public DbSet<Calificacion> Calificaciones => Set<Calificacion>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Evita borrados en cascada que provoquen ciclos en SQL Server.
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Grupo).WithMany(g => g.Asignaciones)
                .HasForeignKey(a => a.GrupoId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Calificacion>()
                .Property(c => c.Valor).HasColumnType("decimal(5,2)");

            // Inscripción: al borrar la asignación se limpian sus inscripciones (cascada);
            // por el lado del alumno se restringe para no crear ciclos de borrado en SQL Server.
            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Alumno).WithMany()
                .HasForeignKey(i => i.AlumnoId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Asignacion).WithMany()
                .HasForeignKey(i => i.AsignacionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
