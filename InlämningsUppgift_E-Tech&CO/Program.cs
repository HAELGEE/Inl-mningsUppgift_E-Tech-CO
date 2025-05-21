using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.Intrinsics.Arm;
using System;
using Microsoft.VisualBasic;

namespace InlämningsUppgift_E_Tech_CO
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new MyDbContext())
            {
                if (db.Customer.Count() == 0)
                {
                    CreateCustomer();
                    db.SaveChanges();                    
                }
                SetLogin();
                Console.CursorVisible = false;
                await RunProgram.RunningProgram();
            }
        }

        static void SetLogin()
        {
            // Sätter alla användares Login till false om nu programmet skulle krasha vid användning så att man inte automatiskt är inloggad
            using (var db = new MyDbContext())
            {
                foreach (var customer in db.Customer)
                {
                    customer.LoggedIn = false;
                }
                db.SaveChanges();
            }
        }

        static void CreateCustomer()
        {
            using (var db = new MyDbContext())
            {
                db.Customer.Add(new Customer
                ("Admin", "Admin", 666, "Admin", "ADMIN", true));
                db.SaveChanges();
            }
        }       

        public static string GetDate()
        {
            DateTime date = DateTime.Now;
            return date.ToString();
        }
    }
}
