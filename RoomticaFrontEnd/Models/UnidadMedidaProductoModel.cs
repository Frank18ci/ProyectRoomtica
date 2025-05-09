using System;
using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class UnidadMedidaProductoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int   Id {  get; set; }
        [Required]
        [Display(Name = "Unidad")]
        public string  Unidad { get; set; }
    }
}
