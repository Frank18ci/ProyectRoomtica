﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ReservaModel
    {
        [Key]
        [Display(Name = "Id Reserva")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Habitación")]
        public int id_habitacion { get; set; }

        [Required]
        [Display(Name = "Trabajador")]
        public int id_trabajador { get; set; }

        [Required]
        [Display(Name = "Tipo de Reserva")]
        public int id_tipo_reserva { get; set; }

        [Required]
        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_ingreso { get; set; }

        [Required]
        [Display(Name = "Fecha de Salida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_salida { get; set; }

        [Required]
        [Display(Name = "Costo de Alojamiento")]
        public double costo_alojamiento { get; set; }
    }

    public class ReservaDTOModel
    {
        [Key]
        [Required]
        [Display(Name = "Reserva")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Habitación")]
        public string? id_habitacion { get; set; }

        [Required]
        [Display(Name = "Trabajador")]
        public string? id_trabajador { get; set; }

        [Required]
        [Display(Name = "Tipo de Reserva")]
        public string? id_tipo_reserva { get; set; }

        [Required]
        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_ingreso { get; set; }

        [Required]
        [Display(Name = "Fecha de Salida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_salida { get; set; }

        [Required]
        [Display(Name = "Costo de Alojamiento")]
        public double? costo_alojamiento { get; set; }
    }
}
