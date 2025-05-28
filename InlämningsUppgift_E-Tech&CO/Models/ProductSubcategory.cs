using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class ProductSubcategory
{
    public int Id { get; set; }                  
    public string? ProductSubcategoryName { get; set; }
    public int? ProductCategoryId { get; set; }     // Foreign Key till ProductCategory
    public virtual ProductCategory? ProductCategory { get; set; }  // Navigation property till ProductCategory
}

