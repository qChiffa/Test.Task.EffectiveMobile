using System.ComponentModel.DataAnnotations;

namespace Test.Task.EffectiveMobile.Models;

public class FilterRequest
{
    [Required(ErrorMessage = "Район доставки обязателен")]
    [RegularExpression(@"^(северный|южный|западный|восточный)$", ErrorMessage = "Недопустимый район")]
    public string District { get; set; }

    [Required(ErrorMessage = "Время первой доставки обязательно")]
    [DataType(DataType.DateTime)]
    public DateTime FirstDeliveryTime { get; set; }
}