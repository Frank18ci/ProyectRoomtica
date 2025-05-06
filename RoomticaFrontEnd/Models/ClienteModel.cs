using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ClienteModel
    {
        [Key]
        [Display(Name ="Id")]
        public int  Id { get; set; }
        [Display(Name = "Primer Nombre")]
        public string? primer_nombre { get; set; }
        [Display(Name = "Segundo Nombre")]
        public string? segundo_nombre { get; set; }
        [Display(Name = "Primer Apellido")]
        public string? primer_apellido { get; set; }
        [Display(Name = "Segundo Apellido")]
        public string? segundo_apellido { get; set; }
        [Display(Name = "Tipo Documento")]
        public int  id_tipo_documento { get; set; }
        [Display(Name = "Numero Documento")]
        public string? numero_documento {  get; set; }
        [Display(Name = "Telefono")]
        public string ? telefono {  get; set; }
        [Display(Name = "Email")]
        public string ? email { get; set; }
        [Display(Name = "Fecha Nacimiento")]
        public string ? fecha_nacimiento { get; set; }
        [Display(Name = "Tipo Nacionalidad")]
        public int  id_tipo_nacionalidad { get; set; }
        [Display(Name = "Tipo Sexo")]
        public int  id_tipo_sexo { get; set; }
    }

    public class ClienteDTOModel
    {
        [Key]
        [Display(Name = "Id")]
        public int? Id { get; set; }
        [Display(Name = "Primer Nombre")]
        public string? primer_nombre { get; set; }
        [Display(Name = "Segundo Nombre")]
        public string? segundo_nombre { get; set; }
        [Display(Name = "Primer Apellido")]
        public string? primer_apellido { get; set; }
        [Display(Name = "Segundo Apellido")]
        public string? segundo_apellido { get; set; }
        [Display(Name = "Tipo Documento")]
        public string? tipo_documento { get; set; }
        [Display(Name = "Numero Documento")]
        public string? numero_documento { get; set; }
        [Display(Name = "Telefono")]
        public string? telefono { get; set; }
        [Display(Name = "Email")]
        public string? email { get; set; }
        [Display(Name = "Fecha Nacimiento")]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? fecha_nacimiento { get; set; }
        [Display(Name = "Tipo Nacionalidad")]
        public string? tipo_nacionalidad { get; set; }
        [Display(Name = "Tipo Sexo")]
        public string? tipo_sexo { get; set; }
    }
}
