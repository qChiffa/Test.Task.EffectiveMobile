using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly string _dataFilePath;
    private readonly FilterRequest _filterRequestSettings;
    private readonly ILogger<DeliveryController> _logger;
    private readonly AppSettings _appSettings;

    public DeliveryController(IOptions<AppSettings> appSettings, ILogger<DeliveryController> logger)
    {
        _dataFilePath = appSettings.Value.DataFilePath;
        _filterRequestSettings = appSettings.Value.FilterRequest;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    [HttpPost("filterFromConfig")]
    public IActionResult FilterOrdersFromConfig()
    {
        _logger.LogInformation("Начало обработки запроса на фильтрации заказов из конфигурации.");

        try
        {
            var validator = new AppSettingsValidator();
            var validationResult = validator.Validate(nameof(AppSettings), _appSettings);
            
            if (validationResult.Failed)
            {
                _logger.LogWarning($"Невалидные данные в конфигурации: {validationResult.FailureMessage}");
                return BadRequest(validationResult.FailureMessage);
            }
            
            var filterRequest = new FilterRequest
            {
                District = _filterRequestSettings.District,
                FirstDeliveryTime = _filterRequestSettings.FirstDeliveryTime,
            };

            var orders = DataReader.ReadOrdersFromFile(_dataFilePath);
            var filteredOrders = OrderFilter.FilterOrders(orders, filterRequest);

            WriteFilteredOrdersToFile(filteredOrders);

            _logger.LogInformation($"Заказы успешно отфильтрованы и записаны в файл. Фильтр: Район={filterRequest.District}, Дата={filterRequest.FirstDeliveryTime}");
            return Ok(filteredOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на фильтрацию заказов из конфигурации.");
            return StatusCode(500, "Произошла ошибка при обработке запроса.");
        }
    }

    
    
    
    [HttpPost("filterFromRequest")]
    public IActionResult FilterOrders([FromBody] FilterRequest filterRequest)
    {
        _logger.LogInformation("Начало обработки запроса на фильтрацию заказов через HTTP запрос.");
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Невалидные данные в запросе.");
            return BadRequest(ModelState);
        }


        try
        {
            var orders = DataReader.ReadOrdersFromFile(_dataFilePath);
            _logger.LogInformation($"Поступил запрос на фильтрацию заказов с параметрами:{filterRequest.District} {filterRequest.FirstDeliveryTime}");
            var filteredOrders = OrderFilter.FilterOrders(orders, filterRequest);
            
        
            WriteFilteredOrdersToFile(filteredOrders);
            
            _logger.LogInformation($"Заказы успешно отфильтрованы и записаны в файл.");
            return Ok(filteredOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса на фильтрацию заказов, через http запрос.");
            
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