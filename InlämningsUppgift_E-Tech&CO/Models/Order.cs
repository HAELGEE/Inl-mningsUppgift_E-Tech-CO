using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Order
{
    public Order(string? name, double? priceAtPurchase)
    {
        Name = name;
        PriceAtPurchase = priceAtPurchase;
        Date = Program.GetDate();
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public double? PriceAtPurchase { get; set; }
    public string? Date { get; set; }    
    public int? CustomerId { get; set; } // En kund kan har flera orderhistorik, En order kan bara ha En kund
    public virtual Customer? Customer { get; set; }
    public virtual Shipping? Shipping { get; set; }
    public virtual ICollection<OrderItem> OrderItem { get; set; }
    public string? PaymentChoice { get; set; }
    public int? TotalAmountPrice { get; set; }
    public int? TotalItems { get; set; }

}
