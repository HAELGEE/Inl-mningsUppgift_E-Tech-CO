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
        ShopId = 1;
    }

    public int ProductCategoryId { get; set; }
    public string? ProductCategoryName { get; set; }
    public virtual ICollection<ProductSubcategory> ProductSubcategories { get; set; } = new List<ProductSubcategory>();
    public int? ShopId { get; set; }     // Foreign Key till ProductCategory
    public virtual Shop? Shop { get; set; }  // Navigation property till ProductCategory
}
