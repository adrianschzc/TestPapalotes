using System.ComponentModel.DataAnnotations;

namespace TestPapalotes.Models
{
    // Roles disponibles en el sistema.
    public enum Rol
    {
        Administrador,
        Profesor,
        Alumno
    }

    // Usuario registrado al que el administrador le asigna un rol.
    public class Usuario
    {
        public int Id { get; set; }

        [Required, Display(Name = "Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required, Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Rol asignado")]
        public Rol Rol { get; set; } = Rol.Alumno;

        // Grupo bajo el que se registra el usuario (requerimiento del panel de administración).
        [Display(Name = "Grupo")]
        public int? GrupoId { get; set; }
        public Grupo? Grupo { get; set; }
    }
}
