using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class ProductCategory
{
    public ProductCategory(string productCategoryName)
    {
        ProductCategoryName = productCategoryName;        
    }

    public int ProductCategoryId { get; set; }
    public string? ProductCategoryName { get; set; }
    public virtual ICollection<ProductSubcategory> ProductSubcategories { get; set; } = new List<ProductSubcategory>();
    public virtual ICollection<Shop> Shop { get; set; } = new List<Shop>();
    //public int? ShopId { get; set; }     // Foreign Key till ProductCategory
    //public virtual Shop? Shop { get; set; }  // Navigation property till ProductCategory
}
