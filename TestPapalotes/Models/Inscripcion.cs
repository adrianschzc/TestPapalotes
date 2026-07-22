namespace TestPapalotes.Models
{
    // Inscripción explícita de un alumno en una asignación (materia + grupo + profesor).
    // Permite al administrador asignar o quitar alumnos de una materia de forma individual,
    // en lugar de que queden inscritos automáticamente por pertenecer al grupo.
    public class Inscripcion
    {
        public int Id { get; set; }

        public int AlumnoId { get; set; }
        public Alumno? Alumno { get; set; }

        public int AsignacionId { get; set; }
        public Asignacion? Asignacion { get; set; }
    }
}
