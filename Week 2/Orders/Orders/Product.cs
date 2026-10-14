using System;
using System.Collections.Generic;
using System.Text;

namespace Products
{

    class Products
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }

        public Decimal Price { get; set; }

        public Products(int OrderId,string ProductName ,decimal Price)
        {
            this.OrderId = OrderId; 
            this.ProductName = ProductName;
            this.Price = Price;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            List<Products> li = new List<Products> {
                new Products(1,"laptop",100000m),
                new Products(2,"Mobile",10000m),
                new Products(3,"HeadPhone",300),
                new Products(4,"PowerBank",5000),
                new Products(5,"Tv",90000)
            };
            
        }


    }
}
