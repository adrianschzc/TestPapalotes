using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Vincula una Materia con un Grupo y el Profesor que la imparte.
    // Los alumnos del grupo son quienes cursan esta materia.
    public class Asignacion
    {
        public int Id { get; set; }

        [Display(Name = "Materia")]
        public int MateriaId { get; set; }
        public Materia? Materia { get; set; }

        [Display(Name = "Grupo")]
        public int GrupoId { get; set; }
        public Grupo? Grupo { get; set; }

        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }
        public Profesor? Profesor { get; set; }

        public ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>();
    }
}
