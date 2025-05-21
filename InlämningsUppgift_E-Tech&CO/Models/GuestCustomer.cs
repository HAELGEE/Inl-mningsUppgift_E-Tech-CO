using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class GuestCustomer
{
    // Denna är skapad för att skapa en "gäst" när man inte är inloggad. Så man kan se items och sånt men man kan inte handla

    public bool IsLoggedIn { get; set; }
    public bool IsAdmin { get; set; }
    public string SettingName { get; set; }   
    

    public void IsLoggedInMethod()
    {
        using (var db = new MyDbContext())
        {
            var customer = db.Customer.Any(x => x.LoggedIn == true);

            if (customer)
                IsLoggedIn = true;
            else
                IsLoggedIn = false;
        }
    }
    public void IsAdminMethod()
    {
        using (var db = new MyDbContext())
        {
            var customer = db.Customer.Any(x => x.LoggedIn == true && x.IsAdmin == true);

            if (customer)
                IsAdmin = true;
            else
                IsAdmin = false;
        }
    }
    public void SettingNameMethod()
    {
        using (var db = new MyDbContext())
        {
            var customer = db.Customer.Where(x => x.LoggedIn == true).SingleOrDefault();

            if (customer.Name != null)
                SettingName = customer.UserName;            
        }
    }
}
