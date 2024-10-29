using System.Globalization;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile;

public static class DataReader
{
    public static List<OrderModel> ReadOrdersFromFile(string filePath)
    {
        var orders = new List<OrderModel>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 4)
            {
                orders.Add(new OrderModel
                {
                    OrderNumber = parts[0],
                    Weight = double.Parse(parts[1], CultureInfo.InvariantCulture),
                    District = parts[2],
                    DeliveryTime = DateTime.ParseExact(parts[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                });
            }
        }

        return orders;
    }
}