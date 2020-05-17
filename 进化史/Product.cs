using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 进化史
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int SupplierID { get; set; }
        public static List<Product> GetSampleProducts()
        {
            return new List<Product>()
        {
            new Product{Name="West Side Story",Price=9.99m,SupplierID=1},
            new Product{Name="Frogs",Price=13.99m,SupplierID=2},
            new Product{Name="Sweeney",Price=10.99m,SupplierID=3}
        };
        }
    }
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public static List<Supplier> GetSampleSupplier()
        {
            return new List<Supplier>
            {
                new Supplier{SupplierID=1,Name="West"},
                new Supplier{SupplierID=2,Name="Frog"},
                new Supplier{SupplierID=3,Name="Sween"}
            };
        }
    }
}
