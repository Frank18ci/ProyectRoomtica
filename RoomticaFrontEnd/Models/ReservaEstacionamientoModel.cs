using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ReservaEstacionamientoModel
    {
        [Key]
        [Display(Name = "Id Reserva")]
        public int? id_reserva { get; set; }

        [Required]
        [Display(Name = "Id Estacionamiento")]
        public int? id_estacionamiento { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int? cantidad { get; set; }

        [Required]
        [Display(Name = "Precio Estacionamiento")]
        public double? precio_estacionamiento { get; set; }
    }

    public class ReservaEstacionamientoDTOModel
    {
        [Key]
        [Required]
        [Display(Name = "Id Reserva")]
        public string? id_reserva { get; set; }

        [Required]
        [Display(Name = "Id Estacionamiento")]
        public string? id_estacionamiento { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int? cantidad { get; set; }

        [Required]
        [Display(Name = "Precio Estacionamiento")]
        public double? precio_estacionamiento { get; set; }
    }
}
