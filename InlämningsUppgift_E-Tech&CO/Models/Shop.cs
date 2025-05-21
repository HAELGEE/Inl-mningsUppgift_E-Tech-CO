using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Shop
{
    public Shop()
    {
        Sold = 0;
        RegularShipping = 200;
        ExpressShipping = 500;
    }

    public int Id { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Name { get; set; }
    public int? Sold { get; set; }
    public int? Quantity { get; set; }
    public double? Price { get; set; }
    public string? ProductInformation { get; set; }
    public virtual ICollection<OrderItem> OrderItem { get; set; }
    public bool? IsActive { get; set; }
    public int? IsActiveCategory { get; set; }
    public int? RegularShipping { get; set; }
    public int? ExpressShipping { get; set; }

}
