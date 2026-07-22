namespace TestPapalotes.Models
{
    // Fila usada en el panel del docente y del alumno: una materia + su calificación.
    public class CalificacionRow
    {
        public int AsignacionId { get; set; }
        public string Materia { get; set; } = string.Empty;
        public string Profesor { get; set; } = string.Empty;
        public decimal? Valor { get; set; }
    }

    // Modelo de la pantalla del docente: datos del alumno buscado + sus materias.
    public class BoletaCalificacionesVM
    {
        public string Boleta { get; set; } = string.Empty;
        public int AlumnoId { get; set; }
        public string AlumnoNombre { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public List<CalificacionRow> Filas { get; set; } = new();
    }
}
