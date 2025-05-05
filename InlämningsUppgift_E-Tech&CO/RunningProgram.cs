using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class RunningProgram
{
    static string categoryName = ""; // Sätter denna här så att jag kan komma åt den i mina metoder    
    static List<string> cartProductsInString = new List<string>();


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
                            loggedinName = customer.Name!;

                    }

                    Console.WriteLine($"What do you want to do?");
                    if (loggedinName == "")
                        Console.WriteLine($"1. Login");
                    else
                        Console.WriteLine($"1. Logout");
                    Console.WriteLine($"2. Register");
                    Console.WriteLine($"3. Profile");
                    Console.WriteLine($"4. Enter Shop");
                    Console.WriteLine($"5. Admin");
                    Console.WriteLine($"6. Quit\n");
                    if (loggedinName != "")
                    {
                        Console.Write($"You are currently logged in as ");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(loggedinName);
                        Console.ResetColor();
                    }
                    else
                        Console.WriteLine("You are currently not logged in");

                    string input = Console.ReadLine()!;

                    validInput = int.TryParse(input, out menu) && menu >= 1 && menu <= 6;

                } while (!validInput);

                db.SaveChanges();

                switch (menu)
                {

                    case 1:
                        if (loggedinName == "")
                        {
                            Console.Clear();

                            Console.Write("Please enter Username: ");
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
                                else
                                {
                                    Console.WriteLine("No username found");
                                    Thread.Sleep(1500);
                                }
                            }
                            ;

                            db.SaveChanges();
                            break;
                        }
                        else
                        {
                            foreach (var customer in db.Customer)
                            {
                                customer.LoggedIn = false;
                            }
                            db.SaveChanges();
                            break;
                        }

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

                            if (int.TryParse(inputCheck, out newAge) && !string.IsNullOrWhiteSpace(inputCheck) && newAge < 0)
                                Console.WriteLine("Must be numbers");
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
                                while (true)
                                {
                                    GUI.DrawWindow("Profile", 5, 10, new List<string>() {
                                                $"1. Firstname: {customer.Name}",
                                                $"2. Lastname: {customer.LastName}",
                                                $"3. Age: {customer.Age}",
                                                $"4. Username: {customer.UserName}",
                                                $"5. Total orders: {customer.OrderHistory.Count()}"
                                            });

                                    GUI.DrawWindow("Update Profile", 38, 10, new List<string>() {
                                                $"1. To change Password",
                                                $"2. To see OrderHistory",
                                                $"3. To see total money spent",
                                                $"B to back"
                                            });
                                    int updateNumber = 0;

                                    Console.SetCursorPosition(5, 18);
                                    string profileCheck = Console.ReadLine()!.ToLower();

                                    if (profileCheck == "b")
                                        break;

                                    if (int.TryParse(profileCheck, out updateNumber) && !string.IsNullOrWhiteSpace(profileCheck))
                                    {

                                    }
                                }
                                break;
                            }
                        }
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

                                if (customerLogedIn)
                                {
                                    GUI.DrawWindowForCart("Shopping Cart", 20, 26, cartProductsInString);
                                }
                                Console.SetCursorPosition(0, 9);
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
                            if (shopMore)
                            {
                                GettingProducts();
                            }
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
        Console.ForegroundColor = ConsoleColor.DarkCyan;
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
            int addToOrder = 0;

            var itemsSubcategory = db.Shop.Where(cn => cn.Category == categoryName)
                                          .GroupBy(sc => sc.SubCategory);

            //var itemToBuy = db.Order.Where(x => x.Id == addToOrder)
            //                            .Join(db.Customer, order => order.Id, customer => customer.Id, (order, customer) => new
            //                            {
            //                                order.Id,                                            
            //                            })
            //                            .Join(db.Shop, order => order.Id, shop => shop.Id, (order, shop, customer) => new
            //                            {
            //                                Order = order,
            //                                Shop = shop,
            //                                Customer = customer
            //                            });
            var itemToBuy = db.Order
                                    .Where(o => o.Id == addToOrder)
                                    .Select(o => new
                                    {
                                        Order = o,
                                        Customer = o.Customer,
                                        Shop = o.Shop
                                    })
                                    .ToList();



            while (true)
            {
                Console.Clear();
                List<Product> cartProducts = new List<Product>();

                // Denna loopen skriver ut listan så man ser vad man kan köpa
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
                        Console.Write($" {item.ProductInformation} ");
                        Console.ResetColor();
                        Console.Write("Price: ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"{item.Price:C}");
                        Console.ResetColor();
                    }
                    Console.WriteLine("---------------------------");
                }


                Console.WriteLine("\nWich Product do you want to buy?");
                Console.WriteLine("\nPress B to back");

                GUI.DrawWindowForCart("Shopping Cart", 20, 26, cartProductsInString); // Före
                Console.SetCursorPosition(0, 31);
                string orderAdd = Console.ReadLine()!.ToLower();
                Console.WriteLine("How many of these?");
                string amountAdd = Console.ReadLine()!.ToLower();
                int amount = 0;

                if (int.TryParse(orderAdd, out addToOrder) && !string.IsNullOrWhiteSpace(orderAdd))
                {
                    //    if (int.TryParse(amountAdd, out amount) && !string.IsNullOrWhiteSpace(amountAdd))
                    //    {
                    foreach (var item in itemToBuy)
                    {
                        if (addToOrder == item.Shop.Id)

                            cartProducts.Add(new Product(item.Shop.Name, amount, item.Shop.Price));
                    }
                    //    }
                }

                foreach (var product in cartProducts)
                {
                    cartProductsInString.Add(new string($"Name: {product.Name.PadRight(23)}\t Amount: {product.Amount}, Price: {product.Price} "));
                }

                //GUI.DrawWindow("Shopping Cart", 20, 26, new List<string>
                //{

                //});
                GUI.DrawWindow("Shopping Cart", 20, 26, cartProductsInString); // Efter

                if (orderAdd == "b")
                    break;

            }
            db.SaveChanges();
        }
    }

}
