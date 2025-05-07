using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TipoComprobanteModel
    {
        [Key]
        [Display(Name = "Id")]
        public int ? id { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string ? tipo { get; set; }
    }
}
