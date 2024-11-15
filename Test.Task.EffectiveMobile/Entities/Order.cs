using System.ComponentModel.DataAnnotations;

namespace Test.Task.EffectiveMobile.Entities;

public class Order
{
    [Required(ErrorMessage = "Номер заказа обязателен")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Номер заказа должен состоять только из цифр")]
    public string OrderNumber { get; set; }
    
    [Required(ErrorMessage = "Вес заказа обязателен")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Вес заказа должен быть больше 0")]
    public double Weight { get; set; }
    
    [Required(ErrorMessage = "Район заказа обязателен")]
    [RegularExpression(@"^(северный|южный|западный|восточный)$", ErrorMessage = "Недопустимый район")]
    public string District { get; set; }
    
    [Required(ErrorMessage = "Время доставки обязательно")]
    [DataType(DataType.DateTime)]
    public DateTime DeliveryTime { get; set; }
}