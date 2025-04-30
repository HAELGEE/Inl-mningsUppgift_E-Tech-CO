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
    }

    public int Id { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Name { get; set; }
    public int? Stock { get; set; }
    public int? Sold { get; set; }
    public double? Price { get; set; }
    public string? ProductInformation { get; set; }
}
