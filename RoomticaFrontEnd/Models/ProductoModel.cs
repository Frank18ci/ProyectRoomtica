using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ProductoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required]
        [Display(Name = "Unidad")]
        public int Unidad { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int Categoria { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [Display(Name = "Precio Unitario")]
        public double PrecioU { get; set; }
    }
}