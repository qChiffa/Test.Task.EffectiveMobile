using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Test.Task.EffectiveMobile.Entities;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController(
    IOptions<AppSettings> appSettings, 
    ILogger<DeliveryController> logger, 
    IDataReader dataReader, 
    IOrderFilter orderFilter,
    IWriteFilterOrders writeFilterOrders) 
    : ControllerBase
{
    private readonly string _dataFilePath = appSettings.Value.DataFilePath;
    private readonly FilterRequest _filterRequestSettings = appSettings.Value.FilterRequest;
    private readonly AppSettings _appSettings = appSettings.Value;

    [HttpPost("filterFromConfig")]
    public IActionResult FilterOrdersFromConfig()
    {
        logger.LogInformation("Начало обработки запроса на фильтрации заказов из конфигурации.");

        try
        {
            var validator = new AppSettingsValidator();
            var validationResult = validator.Validate(nameof(AppSettings), _appSettings);

            if (validationResult.Failed)
            {
                logger.LogWarning($"Невалидные данные в конфигурации: {validationResult.FailureMessage}");
                return BadRequest(validationResult.FailureMessage);
            }

            var filterRequest = new FilterRequest
            {
                District = _filterRequestSettings.District,
                FirstDeliveryTime = _filterRequestSettings.FirstDeliveryTime,
            };

            var orders = dataReader.ReadOrdersFromFile(_dataFilePath);
            var filteredOrders = orderFilter.FilterOrders(orders, filterRequest);

            writeFilterOrders.WriteFilteredOrdersToFile(filteredOrders);
            
            logger.LogInformation(
                $"Заказы успешно отфильтрованы и записаны в файл. Фильтр: Район={filterRequest.District}, Дата={filterRequest.FirstDeliveryTime}");
            return Ok(filteredOrders);
        }
        catch (FileNotFoundException ex)
        {
            logger.LogError(ex, "Файл с заказами не найден.");
            return NotFound("Файл с заказами не найден.");
        }
        catch (IOException ex)
        {
            logger.LogError(ex, "Ошибка при чтении файла с заказами.");
            return StatusCode(500, "Ошибка при чтении файла с заказами.");
        }
    }

    
    
    
    [HttpPost("filterFromRequest")]
    public IActionResult FilterOrders([FromBody] FilterRequest filterRequest)
    {
        logger.LogInformation("Начало обработки запроса на фильтрацию заказов через HTTP запрос.");
        
        try
        {
            var orders = dataReader.ReadOrdersFromFile(_dataFilePath);
            logger.LogInformation($"Поступил запрос на фильтрацию заказов с параметрами:{filterRequest.District} {filterRequest.FirstDeliveryTime}");
            var filteredOrders = orderFilter.FilterOrders(orders, filterRequest);
            
        
            writeFilterOrders.WriteFilteredOrdersToFile(filteredOrders);
            
            logger.LogInformation($"Заказы успешно отфильтрованы и записаны в файл.");
            return Ok(filteredOrders);
        }
        catch (FileNotFoundException ex)
        {
            logger.LogError(ex, "Файл с заказами не найден.");
            return NotFound("Файл с заказами не найден.");
        }
        catch (IOException ex)
        {
            logger.LogError(ex, "Ошибка при чтении файла с заказами.");
            return StatusCode(500, "Ошибка при чтении файла с заказами.");
        }
    }
}