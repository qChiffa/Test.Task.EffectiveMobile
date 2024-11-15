using Test.Task.EffectiveMobile.Entities;

namespace Test.Task.EffectiveMobile;

public interface IWriteFilterOrders
{
    void WriteFilteredOrdersToFile(List<Order> filteredOrders);
}


public class WriteFilterOrders : IWriteFilterOrders
{
    public void WriteFilteredOrdersToFile(List<Order> filteredOrders)
    {
        var lines = new List<string>();
        foreach (var order in filteredOrders)
        {
            lines.Add(
                $"Номер заказа: {order.OrderNumber}, Вес: {order.Weight}кг, Район: {order.District}, Время Доставки:{order.DeliveryTime:yyyy-MM-dd HH:mm:ss}");
        }

        File.WriteAllLines("Data/_deliveryOrder.csv", lines);
    }
}   