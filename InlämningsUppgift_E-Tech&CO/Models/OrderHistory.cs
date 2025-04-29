using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class OrderHistory
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ShopId { get; set; }
    public virtual ICollection<Customer> Customer { get; set; } = new List<Customer>(); // En kund kan har flera orderhistorik
    public virtual ICollection<Shop> Shop { get; set; } = new List<Shop>(); // Måste vara en många till många relation när det gäller historik och shop

}
