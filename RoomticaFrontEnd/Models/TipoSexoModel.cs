using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TipoSexoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int  Id { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public string  tipo { get; set; }
    }
}
