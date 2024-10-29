Приложение поддерживает ввод параметров как через http запрос , так и через переменные среды.
Для использования переменных среды, настройте файл appsettings.json , и укажите в нём нужны для фильтрации данные ("District" и "FirstDeliveryTime"), далее в swagger отправьте запрос на filterFromConfig
Для использования передачи параметров через http запрос, в swagger выберите filterFromRequest , и там также укажите нужные для фильтрации данные ("District" и "FirstDeliveryTime") как показано в примере:
{
  "district": "западный",
  "firstDeliveryTime": "2023-10-01T10:00:00"
}
