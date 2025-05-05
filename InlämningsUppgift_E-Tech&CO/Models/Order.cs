using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Order
{
    public Order(string? name, double? price)
    {
        Name = name;
        Price = price;
        Date = Program.GetDate();
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public string? Date { get; set; }
    public int? CustomerId { get; set; } // En kund kan har flera orderhistorik, En order kan bara ha En kund
    public virtual Customer? Customer { get; set; }
    public int? ShippingId { get; set; }
    public virtual Shipping? Shipping { get; set; }
    public int? ShopId { get; set; } 
    public virtual Shop? Shop { get; set; }
    public string? PaymentChoice { get; set; }
    public virtual ICollection<ShoppingCart> ShoppingCart { get; set; } = new List<ShoppingCart>();
    public int? TotalAmount { get; set; }

}
