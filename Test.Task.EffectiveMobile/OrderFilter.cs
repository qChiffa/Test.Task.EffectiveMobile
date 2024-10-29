using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile;

public static class OrderFilter
{
    public static List<OrderModel> FilterOrders(List<OrderModel> orders, FilterRequest filterRequest)
    {
        var filteredOrders = orders
            .Where(o => o.District.Equals(filterRequest.District, StringComparison.OrdinalIgnoreCase))
            .Where(o => o.DeliveryTime >= filterRequest.FirstDeliveryTime && o.DeliveryTime <= filterRequest.FirstDeliveryTime.AddMinutes(30))
            .ToList();

        return filteredOrders;
    }
}