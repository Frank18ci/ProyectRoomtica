using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class RolTrabajadorModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public string? Rol { get; set; }
    }
}
