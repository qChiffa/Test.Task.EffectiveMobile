using System.ComponentModel.DataAnnotations;

namespace Test.Task.EffectiveMobile.Models;

public class OrderModel
{
    [Required(ErrorMessage = "Номер заказа обязателен")]
    public string OrderNumber { get; set; }
    
    [Range(0.1, double.MaxValue, ErrorMessage = "Вес заказа должен быть больше 0")]
    public double Weight { get; set; }
    
    [Required(ErrorMessage = "Район заказа обязателен")]
    public string District { get; set; }
    
    [Required(ErrorMessage = "Время доставки обязательно")]
    public DateTime DeliveryTime { get; set; }
    
}
