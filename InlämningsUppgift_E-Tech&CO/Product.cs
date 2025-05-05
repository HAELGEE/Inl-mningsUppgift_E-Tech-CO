using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
public class Product
{
    public Product(string? name, int? amount, double? price)
    {
        Name = name;
        Amount = amount;
        Price = price;
    }

    public string? Name { get; set; }
    public int? Amount { get; set; }
    public double? Price { get; set; }
}
