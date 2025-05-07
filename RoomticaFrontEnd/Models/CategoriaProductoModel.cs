using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class CategoriaProductoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; } = string.Empty;
    }
}
