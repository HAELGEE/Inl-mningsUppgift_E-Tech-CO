using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
public class OrderProduct
{
    public OrderProduct(string? name, int? amount, double? price)
    {
        Name = name;
        Amount = amount;
        Price = price;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Amount { get; set; }
    public double? Price { get; set; }
    public int? OrderId { get; set; }
}
