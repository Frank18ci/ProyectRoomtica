using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TipoEstacionamientoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int  Id { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string ? Tipo { get; set; }
        [Required]
        [Display(Name = "Costo")]
        public double  Costo { get; set; }
    }
}
