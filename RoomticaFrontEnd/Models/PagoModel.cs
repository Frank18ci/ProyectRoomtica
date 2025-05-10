using System;
using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class PagoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Id Reserva")]
        public int id_reserva { get; set; }

        [Required]
        [Display(Name = "Id Tipo Comprobante")]
        public int id_tipo_comprobante { get; set; }

        [Required]
        [Display(Name = "IGV")]
        public double igv { get; set; }

        [Required]
        [Display(Name = "Total Pago")]
        public double total_pago { get; set; }

        [Required]
        [Display(Name = "Fecha Emisión")]                
        public string? fecha_emision { get; set; }

        [Required]
        [Display(Name = "Fecha Pago")]        
        public string? fecha_pago { get; set; }
    }

    public class PagoDTOModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public string? id_reserva { get; set; }

        [Required]
        [Display(Name = "Tipo Comprobante")]
        public string? id_tipo_comprobante { get; set; }

        [Required]
        [Display(Name = "IGV")]        
        public double igv { get; set; }

        [Required]
        [Display(Name = "Total Pago")]       
        public double total_pago { get; set; }

        [Required]
        [Display(Name = "Fecha Emisión")]       
        public string? fecha_emision { get; set; }

        [Required]
        [Display(Name = "Fecha Pago")]       
        public string? fecha_pago { get; set; }
    }
}
