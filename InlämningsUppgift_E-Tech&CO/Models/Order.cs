using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Order
{
    public Order()
    {               
        Date = Program.GetDate();       
    }
    public int Id { get; set; }
    public string? Name { get; set; }    
    public string? Date { get; set; }    
    public int? CustomerId { get; set; } // En kund kan har flera orderhistorik, En order kan bara ha En kund
    public string? Shipping {  get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
    public string? PaymentChoice { get; set; }
    public double? TotalAmountPrice { get; set; }
    public int? TotalItems { get; set; }
    public List<Product>? Products { get; set; } = new List<Product>();
    public int? Zipcode { get; set; }
    public string? City { get; set; }
    public string? Adress { get; set; }
    public int? ShippingFee { get; set; }
}
