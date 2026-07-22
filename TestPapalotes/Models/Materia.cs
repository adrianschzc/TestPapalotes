using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Materia / asignatura que se imparte en la universidad.
    public class Materia
    {
        public int Id { get; set; }

        [Required, Display(Name = "Clave")]
        public string Clave { get; set; } = string.Empty;

        [Required, Display(Name = "Nombre de la materia")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Créditos")]
        public int Creditos { get; set; }

        public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
    }
}
