using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TipoEstacionamientoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string ? tipo { get; set; }
        [Required]
        [Display(Name = "Costo")]
        public double ? costo { get; set; }
    }
}
