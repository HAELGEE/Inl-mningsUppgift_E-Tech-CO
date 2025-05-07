using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Shipping
{
    public int Id { get; set; }
    public string? ShippingType { get; set; }
    public string? Address { get; set; }
    public string? Name { get; set; }
    public int? PhoneNumber { get; set; }
    public int? OrderId { get; set; }
    public virtual Order? Order { get; set; }
}
