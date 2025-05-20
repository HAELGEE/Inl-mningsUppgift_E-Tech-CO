using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class GuestCustomer : ICustomer
{
    // Denna är skapad för att skapa en "gäst" när man inte är inloggad. Så man kan se items och sånt men man kan inte handla
    public Customer Customer { get; set; }
    public bool IsLoggedIn { get; set; }
    public bool IsAdmin { get; set; }
    public string SettingName { get; set; }
    public GuestCustomer()
    {
        GettingCustomer();
        IsLoggedInMethod();
        IsAdminMethod();
        SettingNameMethod();
    }

    void GettingCustomer()
    {
        using (var db = new MyDbContext())
        {
            var customer = db.Customer.Where(x => x.LoggedIn == true).SingleOrDefault();

            customer = new Customer(customer.Name, customer.LastName, customer.Age, customer.UserName, customer.Password, customer.IsAdmin);
        }
    }
    
    void IsLoggedInMethod()
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
    void IsAdminMethod()
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
    void  SettingNameMethod()
    {
        using (var db = new MyDbContext())
        {
            var customer = db.Customer.Where(x => x.LoggedIn == true).SingleOrDefault();

            SettingName = customer.UserName;
        }
    }

}
