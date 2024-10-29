using Microsoft.AspNetCore.Mvc;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly string dataFilePath;
    private readonly FilterRequest filterRequest;
    private readonly ILogger<DeliveryController> _logger;

    public DeliveryController(IConfiguration configuration, ILogger<DeliveryController> logger)
    {
        dataFilePath = configuration["DataFilePath"];
        filterRequest = new FilterRequest
        {
            District = configuration["FilterRequest:_cityDistrict"],
            FirstDeliveryTime = DateTime.Parse(configuration["FilterRequest:_firstDeliveryDateTime"])
        };
        _logger = logger;
    }

    [HttpPost("filter")]
    public IActionResult FilterOrders()
    {
        _logger.LogInformation("Начало обработки запроса на фильтрацию заказов.");
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидные данные в запросе.");
            return BadRequest(ModelState);
        }


        try
        {
            var orders = DataReader.ReadOrdersFromFile(dataFilePath);
            _logger.LogInformation($"Поступил запрос на фильтрацию заказов с параметрами:{filterRequest.District} {filterRequest.FirstDeliveryTime}");
            var filteredOrders = OrderFilter.FilterOrders(orders, filterRequest);
            
        
            WriteFilteredOrdersToFile(filteredOrders);
            
            _logger.LogInformation("Заказы успешно отфильтрованы и записаны в файл.");
            return Ok(filteredOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на фильтрацию заказов.");
            
            return StatusCode(500, "Произошла ошибка при обработке запроса.");
        }
    }

    private void WriteFilteredOrdersToFile(List<OrderModel> filteredOrders)
    {
        var lines = new List<string>();
        foreach (var order in filteredOrders)
        {
            lines.Add($"Номер заказа: {order.OrderNumber}, Вес: {order.Weight}кг, Район: {order.District}, Время Доставки:{order.DeliveryTime:yyyy-MM-dd HH:mm:ss}");
        }

        System.IO.File.WriteAllLines("Data/_deliveryOrder.csv", lines);
    }
}