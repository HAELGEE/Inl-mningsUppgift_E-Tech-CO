using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Order
{
    public int Id { get; set; }
    public virtual ICollection<Customer> Customer { get; set; } = new List<Customer>(); // En kund kan har flera orderhistorik
    public virtual ICollection<Shop> Shop { get; set; } = new List<Shop>();
}
