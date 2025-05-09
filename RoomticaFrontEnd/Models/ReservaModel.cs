using System;
using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ReservaModel
    {
        [Key]
        [Display(Name = "Id Reserva")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Habitación")]
        public int? id_habitacion { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int? id_cliente { get; set; }

        [Required]
        [Display(Name = "Trabajador")]
        public int? id_trabajador { get; set; }

        [Required]
        [Display(Name = "Tipo de Reserva")]
        public int? id_tipo_reserva { get; set; }

        [Required]
        [Display(Name = "Fecha de Ingreso")]
        public string? fecha_ingreso { get; set; }

        [Required]
        [Display(Name = "Fecha de Salida")]
        public string? fecha_salida { get; set; }

        [Required]
        [Display(Name = "Costo de Alojamiento")]
        public double? costo_alojamiento { get; set; }
    }

    public class ReservaDTOModel
    {
        [Key]
        [Required]
        [Display(Name = "Id Reserva")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Habitación")]
        public string? id_habitacion { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public string? id_cliente { get; set; }

        [Required]
        [Display(Name = "Trabajador")]
        public string? id_trabajador { get; set; }

        [Required]
        [Display(Name = "Tipo de Reserva")]
        public string? id_tipo_reserva { get; set; }

        [Required]
        [Display(Name = "Fecha de Ingreso")]        
        public string? fecha_ingreso { get; set; }

        [Required]
        [Display(Name = "Fecha de Salida")]        
        public string? fecha_salida { get; set; }

        [Required]
        [Display(Name = "Costo de Alojamiento")]
        public double? costo_alojamiento { get; set; }
    }
}
