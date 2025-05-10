using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class EstadoHabitacionModel
    {
        [Key]
        [Display(Name = "Id")]
        public int  id { get; set; }
        [Required]
        [Display(Name = "Estado Habitacion")]
        public string ? estado_habitacion { get; set; }
    }
}
