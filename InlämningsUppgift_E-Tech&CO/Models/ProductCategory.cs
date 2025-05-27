using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class ProductCategory
{
    public int ProductCategoryId { get; set; }
    public string? ProductCategoryName { get; set; }
    public virtual ICollection<ProductSubcategory> ProductSubcategories { get; set; }
    public virtual ICollection<Shop> Shop { get; set; } = new List<Shop>();
    
}
