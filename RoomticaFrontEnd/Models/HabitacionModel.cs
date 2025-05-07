using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class HabitacionModel
    {
        [Key]
        [Display(Name = "Id")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Número")]
        public string? numero { get; set; }

        [Required]
        [Display(Name = "Piso")]
        public string? piso { get; set; }

        [Required]
        [Display(Name = "Precio Diario")]
        public double? precio_diario { get; set; }

        [Required]
        [Display(Name = "Tipo de Habitación")]
        public int? id_tipo { get; set; }

        [Required]
        [Display(Name = "Estado de Habitación")]
        public int? id_estado { get; set; }
    }

    public class HabitacionDTOModel
    {
        [Key]
        [Display(Name = "Id")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Número")]
        public string? numero { get; set; }

        [Required]
        [Display(Name = "Piso")]
        public string? piso { get; set; }

        [Required]
        [Display(Name = "Precio Diario")]
        public double? precio_diario { get; set; }

        [Required]
        [Display(Name = "Tipo de Habitación")]
        public string? id_tipo { get; set; }

        [Required]
        [Display(Name = "Estado de Habitación")]
        public string? id_estado { get; set; }
    }
}
