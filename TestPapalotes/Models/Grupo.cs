using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Grupo escolar al que pertenecen los alumnos.
    public class Grupo
    {
        public int Id { get; set; }

        [Required, Display(Name = "Clave del grupo")]
        public string Clave { get; set; } = string.Empty;

        [Required, Display(Name = "Nombre del grupo")]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
    }
}
