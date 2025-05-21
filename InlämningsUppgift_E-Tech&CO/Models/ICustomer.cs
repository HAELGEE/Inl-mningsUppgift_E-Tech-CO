using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal interface ICustomer
{
    public bool LoggedIn { get; set; }
    public bool IsAdmin { get; set; }
    public string UserName { get; set; }
}
