using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ConsumoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Id Reserva")]
        public int id_reserva { get; set; }

        [Required]
        [Display(Name = "Id Producto")]
        public int id_producto { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        [Required]
        [Display(Name = "Precio Venta")]
        public double precio_venta { get; set; }
    }
    public class ConsumoDTOModel
    {
        [Key]
        [Required]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public string? reserva { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public string? producto { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        [Required]
        [Display(Name = "Precio Venta")]
        public double precio_venta { get; set; }
    }
}
