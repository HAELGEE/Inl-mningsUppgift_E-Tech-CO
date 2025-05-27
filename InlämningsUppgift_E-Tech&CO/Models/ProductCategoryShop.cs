using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class ProductCategoryShop
{
    public int Id { get; set; }
    public int ProductCategoryId { get; set; }
    public virtual ProductCategory ProductCategory { get; set; }
    public int ShopId { get; set; }
    public virtual Shop Shop { get; set; }
}
