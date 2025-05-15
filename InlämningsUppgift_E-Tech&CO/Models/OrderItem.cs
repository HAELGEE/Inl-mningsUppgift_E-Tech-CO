using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int ShopId { get; set; }
    public virtual Shop Shop { get; set; }
    public int? Quantity { get; set; }    
}
