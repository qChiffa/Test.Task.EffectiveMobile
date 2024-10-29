using System.ComponentModel.DataAnnotations;

namespace Test.Task.EffectiveMobile.Models;

public class FilterRequest
{
    [Required(ErrorMessage = "Район доставки обязателен")]
    public string District { get; set; }
    [Required(ErrorMessage = "Время первой доставки обязательно")]
    public DateTime FirstDeliveryTime { get; set; }
}