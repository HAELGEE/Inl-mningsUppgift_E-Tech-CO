using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal interface ICustomer
{
    public bool IsLoggedIn { get; }
    public bool IsAdmin { get; }
    public string SettingName { get; }
}
