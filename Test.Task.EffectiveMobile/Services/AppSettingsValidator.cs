using Microsoft.Extensions.Options;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile;

public class AppSettingsValidator : IValidateOptions<AppSettings>
{
    public ValidateOptionsResult Validate(string name, AppSettings options)
    {
        if (string.IsNullOrEmpty(options.DataFilePath))
        {
            return ValidateOptionsResult.Fail("Путь к файлу данных не указан");
        }

        if (options.FilterRequest == null || string.IsNullOrEmpty(options.FilterRequest.District) 
                                          || options.FilterRequest.FirstDeliveryTime == default)
        {
            return ValidateOptionsResult.Fail("Некорректные настройки фильтра заказов");
        }

        return ValidateOptionsResult.Success;
    }
}