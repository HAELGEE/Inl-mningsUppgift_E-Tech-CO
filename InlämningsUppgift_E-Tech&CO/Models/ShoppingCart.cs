using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class ShoppingCart
{
    public int Id { get; set; }
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
    public int? TotalPrice { get; set; }
    public int? TotalItems { get; set; }

    // BEHÖVER NOG INTE HA DENNA ?
}
