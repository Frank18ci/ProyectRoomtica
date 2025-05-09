using System;
using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class TrabajadorModel
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Primer Nombre")]
        public string? primer_nombre { get; set; }

        [Required]
        [Display(Name = "Segundo Nombre")]
        public string? segundo_nombre { get; set; }

        [Required]
        [Display(Name = "Primer Apellido")]
        public string? primer_apellido { get; set; }

        [Required]
        [Display(Name = "Segundo Apellido")]
        public string? segundo_apellido { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string? username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string? password { get; set; }

        [Required]
        [Display(Name = "Sueldo")]
        public double sueldo { get; set; }

        [Required]
        [Display(Name = "Tipo Documento")]
        public int id_tipo_documento { get; set; }

        [Required]
        [Display(Name = "Numero Documento")]
        public string? numero_documento { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string? telefono { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string? email { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public int id_rol { get; set; }
    }

    public class TrabajadorDTOModel
    {
        [Key]
        [Required]
        [Display(Name = "Id")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "Primer Nombre")]
        public string? primer_nombre { get; set; }

        [Required]
        [Display(Name = "Segundo Nombre")]
        public string? segundo_nombre { get; set; }

        [Required]
        [Display(Name = "Primer Apellido")]
        public string? primer_apellido { get; set; }

        [Required]
        [Display(Name = "Segundo Apellido")]
        public string? segundo_apellido { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string? username { get; set; }

        [Required]
        [Display(Name = "Password")]       
        public string? password { get; set; }

        [Required]
        [Display(Name = "Sueldo")]
        public double? sueldo { get; set; }

        [Required]
        [Display(Name = "Tipo Documento")]
        public string? tipo_documento { get; set; }

        [Required]
        [Display(Name = "Numero Documento")]
        public string? numero_documento { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string? telefono { get; set; }

        [Required]
        [Display(Name = "Email")]        
        public string? email { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public string? rol { get; set; }
    }
}
