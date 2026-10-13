using System;
using System.Globalization;
using System.Collections.Generic;

abstract class Orders 
{
    public int OrderId { get; }
    public string CustomerName { get; }
    public decimal TotalOrder { get; }

    public abstract decimal AdditionalCharges();



    protected Orders(int OrderId, string CustomerName , decimal TotalOrder)
    {

        if (OrderId <= 0)
        {
            throw new ArgumentOutOfRangeException("order id must be positive");
        }
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            throw new ArgumentNullException("Name not Be Null");
        }

        if (TotalOrder <= 0)
        {
            throw new ArgumentOutOfRangeException("Order must be Positive");
        }

        this.OrderId = OrderId;
        this.CustomerName = CustomerName;
        this.TotalOrder = TotalOrder;
    }

    public decimal FinalAmount => TotalOrder + AdditionalCharges();
}

class StoresOrder : Orders
{
    private const Decimal HandlingFee = 0.02m;

    public StoresOrder (int OrderId,string CustomerName,decimal TotalOrder) : base (OrderId ,CustomerName,TotalOrder)
    {

    }

    public override decimal  AdditionalCharges()
    {
        return Math.Round(TotalOrder * HandlingFee, 2, MidpointRounding.AwayFromZero);
    }
    
}
class OnlineOrders : Orders
{
    private const Decimal DeliveryCharge = 0.05m;

    public OnlineOrders(int OrderId, string CustomerName, decimal TotalOrder) : base(OrderId, CustomerName, TotalOrder)
    {

    }

    public override decimal AdditionalCharges()
    {
        return Math.Round(TotalOrder * DeliveryCharge, 2, MidpointRounding.AwayFromZero);
    }

}



public class Program { 
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CultureInfo culture = CultureInfo.GetCultureInfo("en-IN");
        List<Orders> order = new List<Orders>
        {
            new OnlineOrders(1,"Chandru",1000m),
            new StoresOrder(2,"Chistofer Nolan",1000m),
            new OnlineOrders(3,"Ana de Armas",2000m)
        };
        Console.WriteLine("ORDER MANAGEMENT SYSTEM");
        Console.WriteLine(new string('=', 35));
        foreach (Orders o in order)
        {
            Console.WriteLine($"Order Type : {o.GetType().Name}");
            Console.WriteLine($"Order ID : {o.OrderId}");
            Console.WriteLine($"CustomerName :  {o.CustomerName}");
            Console.WriteLine($"Total Order :  {o.TotalOrder.ToString("C", culture)}");
            Console.WriteLine($"Additional Charges :  {o.AdditionalCharges().ToString("c",culture)}");
            Console.WriteLine($"Final Amount : {o.FinalAmount.ToString("c",culture)}");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
