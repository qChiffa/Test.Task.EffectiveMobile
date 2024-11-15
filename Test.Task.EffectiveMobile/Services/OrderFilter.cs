using Test.Task.EffectiveMobile.Entities;
using Test.Task.EffectiveMobile.Models;

namespace Test.Task.EffectiveMobile;

public interface IOrderFilter
{
    List<Order> FilterOrders(List<Order> orders, FilterRequest filterRequest);
}


public  class OrderFilter : IOrderFilter
{
    public  List<Order> FilterOrders(List<Order> orders, FilterRequest filterRequest)
    {
        var filteredOrders = orders
            .Where(o => o.District.Equals(filterRequest.District, StringComparison.OrdinalIgnoreCase))
            .Where(o => o.DeliveryTime >= filterRequest.FirstDeliveryTime && o.DeliveryTime <= filterRequest.FirstDeliveryTime.AddMinutes(30))
            .ToList();

        return filteredOrders;
    }
}