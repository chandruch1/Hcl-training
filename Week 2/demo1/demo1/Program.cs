
using System;
using System.ComponentModel.DataAnnotations;
enum Type { 
    SmallOrder,
    MediumOrder,
    LargeOrder,
    BulkOrder

}

class Product
{
    public int Id { get; set; }

    private string name;

    public string Name{ get { return name; }
        set
        {
            name = value;
        }
    }
}

class Products
{
    public int OrderId { get; set; } = 0;
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public int Quanity { get; set; } = 0;

    public decimal Price { get; set; } 

    public Type Status { get; set; }

    void Set()
    {
        Console.WriteLine("Enter your Product Id:");
        do
        {
            OrderId = int.Parse(Console.ReadLine());
            if (OrderId < 0)
            {
                Console.WriteLine("Enter ID Again");
            }
        }
        while (OrderId < 0);

        Console.WriteLine("Enter your Name:");
        do
        {
            CustomerName = Console.ReadLine();
            if (CustomerName == "")
            {
                Console.WriteLine("Enter your Name Again");
            }
        }
        while (CustomerName == "");
        

        Console.WriteLine("Enter your Product Name:");
        do
        {
            ProductName = Console.ReadLine();
            if (ProductName == "")
            {
                Console.WriteLine("Enter Product Name Again");
            }
        }
        while (ProductName == "");
        

        Console.WriteLine("Enter your Quantity:");
        do
        {
            Quanity = int.Parse(Console.ReadLine());
            if (Quanity < 0)
            {
                Console.WriteLine("Enter Quantinty Again");
            }
        }
        while (Quanity < 0);


        decimal Pr;

        Console.WriteLine("Enter Price:");

        while (true)
        {
            

            if (decimal.TryParse(Console.ReadLine(), out Pr) && Price >= 0)
            {
                Price = Pr;
                break;
            }

            Console.WriteLine("Invalid price. Enter again");
        }

    }

    void Disp()
    {
        Console.WriteLine();
        Console.WriteLine("--- Order  Summary ---");
        Console.WriteLine("Order ID: " + OrderId);
        Console.WriteLine("Customer Name: " + CustomerName);
        Console.WriteLine("Product Name: " + ProductName);
        Console.WriteLine("Quantity: " + Quanity);
        Console.WriteLine("Price: " + Price);
        Console.WriteLine("Total Amount: " + (Quanity * Price));
        if (Price < 1000)
        {
            Status = Type.SmallOrder;
        }
        else if (Price < 5000 && Price >= 1000) Status = Type.MediumOrder;

        else if (Price < 10000 && Price >= 5000) Status = Type.LargeOrder;

        else Status = Type.BulkOrder;

        Console.WriteLine(Status);
    }

    public static void Main(string[] args)
    {
            //Products obj = new Products();

            //obj.Set();
            //obj.Disp();

            Product obj = new Product();
            obj.Name = "chandru";
            Console.WriteLine(obj.Name);


    }
}
