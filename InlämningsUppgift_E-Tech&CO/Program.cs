using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.Intrinsics.Arm;
using System;
using Microsoft.VisualBasic;

namespace InlämningsUppgift_E_Tech_CO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetLogin();
            Console.CursorVisible = false;
            RunningProgram.RunProgram();
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

        public static string GetDate()
        {
            DateTime date = DateTime.Now;
            return date.ToString();
        }

        public static void GetDatabaseStuff()
        {
            using (var db = new MyDbContext())
            {

                db.Shop.Add(new Shop
                {
                    //Category = category,
                    //Name = productName,
                    //Quantity = stock,
                    //ProductInformation = information,
                    //SubCategory = subCategory,
                    //Price = productPrice
                });

                db.SaveChanges();

            }
        }
    }
}
