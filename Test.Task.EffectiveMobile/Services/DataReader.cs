using System.Globalization;
using Test.Task.EffectiveMobile.Entities;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile;

public interface IDataReader
{
    List<Order> ReadOrdersFromFile(string filePath);
}


public  class DataReader : IDataReader
{
    public  List<Order> ReadOrdersFromFile(string filePath)
    {
        var orders = new List<Order>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 4)
            {
                orders.Add(new Order
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