using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class EstacionamientoModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Lugar")]
        public string? lugar { get; set; }

        [Required]
        [Display(Name = "Largo")]
        public string? largo { get; set; }

        [Required]
        [Display(Name = "Alto")]
        public string? alto { get; set; }

        [Required]
        [Display(Name = "Ancho")]
        public string? ancho { get; set; }

        [Required]
        [Display(Name = "Tipo Estacionamiento")]
        public int id_tipo_estacionamiento { get; set; }
    }

    public class EstacionamientoDTOModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Lugar")]
        public string? lugar { get; set; }

        [Required]
        [Display(Name = "Largo")]
        public string? largo { get; set; }

        [Required]
        [Display(Name = "Alto")]
        public string? alto { get; set; }

        [Required]
        [Display(Name = "Ancho")]
        public string? ancho { get; set; }

        [Required]
        [Display(Name = "Tipo Estacionamiento")]
        public string? tipo_estacionamiento { get; set; }
    }
}
