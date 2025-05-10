using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TipoHabitacionModel
    {
        [Key]
        [Display(Name = "Id")]
        public int  Id { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string ? Tipo { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Descripccion")]
        public string ? descripccion { get; set; } = string.Empty;
        
    }
}
