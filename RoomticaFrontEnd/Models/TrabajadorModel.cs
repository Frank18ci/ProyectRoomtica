using System;

namespace RoomticaFrontEnd.Models
{
    public class TrabajadorModel
    {
        public int ? id { get; set; }
        public string ? primer_nombre { get; set; }
        public string ? segundo_nombre { get; set; }
        public string ? primer_apellido { get; set; }
        public string ? segundo_apellido { get; set; }
        public string ? username { get; set; }
        public string ? password { get; set; }
        public double ? sueldo { get; set; }
        public int ? id_tipo_documento { get; set; }
        public string ? numero_documento { get; set; }
        public string ? telefono {  get; set; }
        public string ? email { get; set; }
        public int ? id_rol {  get; set; }
    }

    public class TrabajadorDTOModel
    {
        public int? id { get; set; }
        public string? primer_nombre { get; set; }
        public string? segundo_nombre { get; set; }
        public string? primer_apellido { get; set; }
        public string? segundo_apellido { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public double? sueldo { get; set; }
        public string? id_tipo_documento { get; set; }
        public string? numero_documento { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? id_rol { get; set; }
    }
}
