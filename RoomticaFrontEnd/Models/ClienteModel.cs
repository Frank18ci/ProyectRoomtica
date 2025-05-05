using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ClienteModel
    {
        public int ? Id { get; set; }
        public string? primer_nombre { get; set; }
        public string? segundo_nombre { get; set; }
        public string? primer_apellido { get; set; }
        public string? segundo_apellido { get; set; }
        public int ? id_tipo_documento { get; set; }
        public string? numero_documento {  get; set; }
        public string ? telefono {  get; set; }
        public string ? email { get; set; }
        public string ? fecha_nacimiento { get; set; }
        public int ? id_tipo_nacionalidad { get; set; }
        public int ? id_tipo_sexo { get; set; }
    }

    public class ClienteDTOModel
    {
        public int? Id { get; set; }
        public string? primer_nombre { get; set; }
        public string? segundo_nombre { get; set; }
        public string? primer_apellido { get; set; }
        public string? segundo_apellido { get; set; }
        public string? id_tipo_documento { get; set; }
        public string? numero_documento { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? fecha_nacimiento { get; set; }
        public string? id_tipo_nacionalidad { get; set; }
        public string? id_tipo_sexo { get; set; }
    }
}
