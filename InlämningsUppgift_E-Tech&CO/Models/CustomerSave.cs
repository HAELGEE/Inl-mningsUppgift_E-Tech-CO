using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class CustomerSave
{
    // Denna används inte. Tänkte bygga ut en WishList men blev inte av.
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Count { get; set; }
    public int? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
}
