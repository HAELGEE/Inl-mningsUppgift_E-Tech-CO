using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class RunningProgram
{
    static string categoryName = ""; // Sätter denna här så att jag kan komma åt den i mina metoder

    public static void RunProgram()
    {
        bool running = true;

        while (running)
        {
            using (var db = new MyDbContext())
            {
                string loggedinName = "";
                int menu = 0;
                bool validInput = false;
                do
                {
                    Console.Clear();
                    WelcomeMessage();       // Skrivet ut ett välkomstmeddelande
                    CompanyName();          // Skriver ut Företagsnamnet

                    foreach (var customer in db.Customer)
                    {
                        if (customer.LoggedIn)
                            loggedinName = customer.Name;
                    }

                    Console.WriteLine($"What do you want to do?");
                    Console.WriteLine($"1. Login");
                    Console.WriteLine($"2. Register");
                    Console.WriteLine($"3. Profile");
                    Console.WriteLine($"4. Enter Shop");
                    Console.WriteLine($"5. Admin");
                    Console.WriteLine($"6. Quit\n");
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
                        string newFirstName = Console.ReadLine()!;
                        Console.Write("Please enter ur Lastname: ");
                        string newLastname = Console.ReadLine()!;

                        int newAge = 0;
                        while (newAge <= 0)
                        {
                            Console.Write("Please enter ur Age: ");
                            string inputCheck = Console.ReadLine()!;

                            if (int.TryParse(inputCheck, out newAge) && !string.IsNullOrWhiteSpace(inputCheck) && newAge > 0)
                                Console.WriteLine("Måste vara siffror");
                        }

                        Console.Write("Please enter UserName: ");
                        string newUserName = Console.ReadLine()!;
                        Console.Write("Please enter ur Password: ");
                        string newPassword = Console.ReadLine()!;

                        db.Customer.Add(new Customer
                        (
                            newFirstName,
                            newLastname,
                            newAge,
                            newUserName,
                            newPassword
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

                        //if (customerLogedIn)
                        //{
                        //Console.WriteLine("Top sellers!");
                        //Console.WriteLine("----------------------");
                        //foreach (var item in topSeller)
                        //{
                        //    Console.WriteLine(item.Sold);
                        //}

                        //Console.WriteLine("----------------------");


                        bool shopMore = true;
                        while (shopMore)
                        {
                            int validNum = 0;
                            while (validNum <= 0 || validNum > 3)
                            {
                                int categoryNum = 0;
                                Console.Clear();
                                foreach (var cat in items)
                                {
                                    categoryNum++;
                                    Console.WriteLine($"{categoryNum}. Category: {cat.Key}");
                                    Console.WriteLine("----------------------");
                                }

                                Console.WriteLine("Press B to back");
                                Console.Write("Wich product do you want to enter?: ");
                                string numberCheck = Console.ReadLine()!.ToLower();

                                if (numberCheck == "b")
                                {
                                    shopMore = false;
                                    break;
                                }

                                if (!int.TryParse(numberCheck, out validNum) || validNum < 1 || validNum > 3)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Must be a number from {categoryNum / categoryNum} to {categoryNum}");
                                    Console.ResetColor();
                                    Thread.Sleep(1500);
                                }
                                else
                                {
                                    if (validNum == 1)
                                        categoryName = "Computer";
                                    else if (validNum == 2)
                                        categoryName = "Phone";
                                    else if (validNum == 3)
                                        categoryName = "Screen";
                                }
                            }

                            Console.Clear();
                            GettingProducts();

                            
                        }
                            //}
                        db.SaveChanges();
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

    static void GettingProducts()
    {
        using (var db = new MyDbContext())
        {
            var itemsSubcategory = db.Shop.Where(cn => cn.Category == categoryName)
                                          .GroupBy(sc => sc.SubCategory);

            int addToOrder = 0;
            while (true)
            {
                Console.Clear();

                foreach (var sub in itemsSubcategory)
                {
                    Console.WriteLine($"Category: {sub.Key}\n----------------------");
                    foreach (var item in sub)
                    {
                        Console.Write($"{item.Id}. Name: {item.Name} Total in stock: ");

                        if (item.Stock > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{item.Stock}");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{item.Stock}");
                        }
                        Console.ResetColor();

                        Console.Write($"Product info:");

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($" {item.ProductInformation} \n");
                        Console.ResetColor();
                    }
                    Console.WriteLine("---------------------------");
                }


                Console.Write("\nWich Product do you want to buy?: ");
                Console.WriteLine("\nPress B to back");

                GUI.DrawWindowForShop("Shopping Cart", 50, 26, new List<string>() { 
                    $"1. Firstname: ",
                    $"2. Lastname: ",
                    $"3. Age: ",
                    $"4. Username: ",
                    $"5. Total Price: ",
                    $"Press C to checkout"
                });
                Console.SetCursorPosition(0, 31);
                string orderAdd = Console.ReadLine()!.ToLower();

                if(int.TryParse(orderAdd, out addToOrder) && !string.IsNullOrWhiteSpace(orderAdd))
                {

                }

                

                if (orderAdd == "b")
                    break;

            }
            db.SaveChanges();
        }
    }

}
