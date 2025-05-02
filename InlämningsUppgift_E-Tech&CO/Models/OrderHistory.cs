using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class OrderHistory
{
    public int Id { get; set; }
    public int? CustomerId { get; set; } // En kund kan har flera orderhistorik, En order kan bara ha En kund
    public virtual Customer? Customer { get; set; }
    public int? ShippingId { get; set; }
    public virtual Shipping? Shipping { get; set; }
    public virtual ICollection<Shop> Shop { get; set; } = new List<Shop>(); // Måste vara en många till många relation när det gäller historik och shop

}
