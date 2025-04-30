using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InlämningsUppgift_E_Tech_CO.Models;
internal class Customer
{
    public Customer(string? name, string? lastName, int? age, string? userName, string? password)
    {
        Name = name;
        LastName = lastName;
        Age = age;
        UserName = userName;
        Password = password;
        Registered = GetDate();
        Logins = 0;
        LoggedIn = true; // sätter denna till true när man registrerat sig så man automatiskt kan gå in i shop för att slippa login
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Registered { get; set; }
    public bool LoggedIn { get; set; }
    public int? Age { get; set; }
    public int? Logins { get; set; }
    public virtual ICollection<CustomerSave> Saves { get; set; } = new List<CustomerSave>(); // En kund kan har flera saves
    public virtual ICollection<OrderHistory> OrderHistory { get; set; } = new List<OrderHistory>(); // En kund kan har flera orderhistorik
    public virtual ICollection<Order> Order { get; set; } = new List<Order>(); // En kund kan har flera orderhistorik

    private string GetDate()
    {
        DateTime date = DateTime.Now;        
        return date.ToString();
    }

}
