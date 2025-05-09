using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TipoDocumentoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public string? Tipo { get; set; }
    }
}
