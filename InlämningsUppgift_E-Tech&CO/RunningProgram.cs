using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class RunningProgram
{
    public static void RunProgram()
    {
        bool running = true;

        while (running)
        {
            using (var db = new MyDbContext())
            {
                string loggedinName = "";
                var menu = 0;
                bool validInput = false;
                do
                {
                    Console.Clear();
                    WelcomeMessage();       // Skrivet ut ett välkomstmeddelande
                    CompanyName();          // Skriver ut Företagsnamnet

                    Console.WriteLine($"What do you wanna do?");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Profile");
                    Console.WriteLine("4. Enter Shop");
                    Console.WriteLine("5. Admin");
                    Console.WriteLine("6. Quit\n");
                    foreach (var customer in db.Customer)
                    {
                        if (customer.LoggedIn)
                            loggedinName = customer.Name;
                    }
                    if (loggedinName != "")
                        Console.WriteLine($"You are currently logged in as {loggedinName}");
                    else
                        Console.WriteLine("You are currently not logged in");

                    string input = Console.ReadLine()!;

                    validInput = int.TryParse(input, out menu) && menu >= 1 && menu <= 6;

                } while (!validInput);


                switch (menu)
                {
                    case 1:
                        Console.Clear();

                        Console.Write("Please enter UserName: ");
                        string userName = Console.ReadLine()!;

                        foreach (var customer in db.Customer)
                        {
                            if (customer.UserName == userName)
                            {
                                Console.Write("Please enter Password: ");
                                string loginPassword = Console.ReadLine()!;

                                if (customer.Password == loginPassword)
                                {
                                    Console.WriteLine("You are now logged in!");
                                    customer.LoggedIn = true;
                                    customer.Logins = customer.Logins + 1;
                                    Thread.Sleep(1500);
                                }
                                else
                                {
                                    Console.WriteLine("Incorret password!");
                                    Thread.Sleep(1500);
                                }
                            }
                        }
                        ;

                        db.SaveChanges();
                        break;

                    case 2:
                        Console.Clear();

                        Console.Write("Please enter ur Firstname: ");
                        string newfirstname = Console.ReadLine()!;
                        Console.Write("Please enter ur Lastname: ");
                        string newlastname = Console.ReadLine()!;
                        int newage = 0;
                        do
                        {
                            Console.Write("Please enter ur Age: ");
                            newage = int.Parse(Console.ReadLine()!);
                            if (newage < 0 || newage > 110)
                                Console.WriteLine("Måste vara siffror");
                        } while (newage < 0 || newage > 110);     // Sätter att man måste vara äldre än 0 år och yngre än 110 annars loopar den om

                        Console.Write("Please enter UserName: ");
                        string newuserName = Console.ReadLine()!;
                        Console.Write("Please enter ur Password: ");
                        string newpassword = Console.ReadLine()!;

                        db.Customer.Add(new Customer
                        (
                            newfirstname,
                            newlastname,
                            newage,
                            newuserName,
                            newpassword
                        ));

                        db.SaveChanges();
                        break;

                    case 3:
                        Console.Clear();

                        foreach (var customer in db.Customer)
                        {
                            if (customer.LoggedIn)
                            {
                                GUI.DrawWindow("Profile", 5, 10, new List<string>() { $"1. Firstname: {customer.Name}", $"2. Lastname: {customer.LastName}", $"3. Age: {customer.Age}", $"4. Username: {customer.UserName}", $"5. Total orders: {customer.OrderHistory.Count()}" });
                            }
                        }
                        Console.ReadKey();
                        break;

                    case 4:
                        Console.Clear();

                        // kontrollerar alla användare om vilken som är inloggad annars kan man inte gå in i affären
                        var topSeller = db.Shop
                            .OrderByDescending(x => x.Sold)
                            .Take(3);

                        var items = db.Shop.GroupBy(x => x.Category);


                        bool customerLogedIn = false;
                        foreach (var customer in db.Customer)
                        {
                            if (customer.LoggedIn)
                                customerLogedIn = true;
                        }

                        if (customerLogedIn)
                        {
                            Console.WriteLine("Top sellers!");
                            Console.WriteLine("----------------------");
                            foreach (var item in topSeller)
                            {
                                Console.WriteLine(item.Sold);
                            }
                            Console.WriteLine("----------------------");
                            foreach (var cat in items)
                            {

                                Console.WriteLine($"Category: {cat.Key}");
                                Console.WriteLine("----------------------");
                                foreach (var item in cat)
                                {
                                    Console.WriteLine($"   ID: {item.Id} \t {item.Name}");
                                }
                            }

                            Console.Write("Wich product do you want to enter?: ");


                            Console.ReadKey();
                        }


                        break;

                    case 5:
                        Console.Clear();

                        Admin.AdminConsol();

                        break;

                    case 6:
                        running = false;
                        Console.WriteLine("Good bye, see you later!");
                        break;
                }
            }
        }
    }
    static void WelcomeMessage()
    {
        Console.WriteLine(@"__          __  _                             _         ");
        Console.WriteLine(@"\ \        / / | |                           | |        ");
        Console.WriteLine(@" \ \  /\  / /__| | ___ ___  _ __ ___   ___   | |_ ___   ");
        Console.WriteLine(@"  \ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \  | __/ _ \  ");
        Console.WriteLine(@"   \  /\  /  __/ | (_| (_) | | | | | |  __/  | || (_) | ");
        Console.WriteLine(@"    \/  \/ \___|_|\___\___/|_| |_| |_|\___|   \__\___/  ");

    }
    static void CompanyName()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@" ______   _______        _                 _____ ____   ");
        Console.WriteLine(@"|  ____| |__   __|      | |       ___     / ____/ __ \  ");
        Console.WriteLine(@"| |__ ______| | ___  ___| |__    ( _ )   | |   | |  | | ");
        Console.WriteLine(@"|  __|______| |/ _ \/ __| '_ \   / _ \/\ | |   | |  | | ");
        Console.WriteLine(@"| |____     | |  __/ (__| | | | | (_>  < | |___| |__| | ");
        Console.WriteLine(@"|______|    |_|\___|\___|_| |_|  \___/\/  \_____\____/  ");
        Console.WriteLine();
        Console.WriteLine();
        Console.ResetColor();
    }

}
