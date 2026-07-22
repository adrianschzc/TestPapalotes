using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Alumno inscrito en un grupo. La boleta es su identificador único.
    public class Alumno
    {
        public int Id { get; set; }

        [Required, Display(Name = "No. de boleta")]
        public string Boleta { get; set; } = string.Empty;

        [Required, Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress, Display(Name = "Correo")]
        public string? Correo { get; set; }

        [Display(Name = "Grupo")]
        public int? GrupoId { get; set; }
        public Grupo? Grupo { get; set; }

        public ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>();
    }
}
