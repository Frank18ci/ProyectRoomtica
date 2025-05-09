using System.ComponentModel.DataAnnotations;
namespace RoomticaFrontEnd.Models
{
    public class CaracteristicaHabitacionModel
    {
        [Key] [Display(Name = "Id")] public int  Id { get; set; } 
        [Required] [Display(Name = "Caracteristica")] public string ? Caracteristica { get; set; }
    }
}
