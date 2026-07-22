using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Calificación de un alumno en una asignación (materia impartida en su grupo).
    public class Calificacion
    {
        public int Id { get; set; }

        [Display(Name = "Alumno")]
        public int AlumnoId { get; set; }
        public Alumno? Alumno { get; set; }

        [Display(Name = "Asignación (materia)")]
        public int AsignacionId { get; set; }
        public Asignacion? Asignacion { get; set; }

        [Range(0, 100), Display(Name = "Calificación")]
        public decimal Valor { get; set; }
    }
}
