using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Profesor / docente que imparte materias.
    public class Profesor
    {
        public int Id { get; set; }

        [Required, Display(Name = "No. de empleado")]
        public string NumEmpleado { get; set; } = string.Empty;

        [Required, Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress, Display(Name = "Correo")]
        public string? Correo { get; set; }

        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
    }
}
