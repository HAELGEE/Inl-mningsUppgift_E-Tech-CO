using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InlämningsUppgift_E_Tech_CO;
internal class RunProgram
{
    static string categoryName = ""; // Sätter denna här så att jag kan komma åt den i mina metoder       
    static List<Product> cartProducts = new List<Product>();
    static List<string> cartProductsInString = new List<string>();
    static double totalAmount = 0;
    static ICustomer isGuest = new Customer();
    public static async Task RunningProgram()
    {
        bool running = true;
        while (running)
        {
            using (var db = new MyDbContext())
            {

                int menu = 0;
                bool validInput = false;
                do
                {
                    Console.Clear();
                    WelcomeMessage();       // Skrivet ut ett välkomstmeddelande
                    CompanyName();          // Skriver ut Företagsnamnet

                    List<string> top3List = new List<string>();

                    var top3 = db.Shop.Where(x => x.IsActive == true)
                        .OrderByDescending(x => x.Sold)
                        .Take(3).ToList();

                    var top3Category = db.Shop
                        .OrderByDescending(x => x.Sold)
                        .Where(x => x.IsActiveCategory != null)
                        .Take(1).SingleOrDefault();

                    // Här kommer logiken för att få fram vilken kategori som har mest sålda produkter
                    var allCategories = db.Shop.GroupBy(x => x.SubCategory);

                    List<(string Category, int? totalSold)> topCategories = new List<(string, int?)>(); // Tillfälligt skapad Lista för att sortera ut

                    foreach (var key in allCategories)
                    {
                        int? counter = 0;
                        foreach (var product in key)
                        {
                            counter += product.Sold;
                        }
                        topCategories.Add((key.Key, counter));
                    }
                    var orderedList = topCategories.OrderByDescending(x => x.totalSold)
                    .Take(3);

                    if (top3Category == null)
                    {
                        top3List.Add($"The List is empty at the moment");
                    }
                    else if (top3Category.IsActiveCategory == 1)
                    {
                        int i = 1;
                        foreach (var item in top3)
                        {
                            top3List.Add($"Nr {i}. Product Name: {item.Name.PadRight(33)} Sold: {item.Sold} Price: {item.Price:C}/unit");
                            i++;
                        }
                    }
                    else if (top3Category.IsActiveCategory == 2)
                    {
                        int i = 1;
                        foreach (var item in orderedList)
                        {
                            top3List.Add($"Nr {i}. Subcategory/Maker: {item.Category}");
                            i++;
                        }
                    }



                    GUI.DrawWindow("Top3", 40, 13, top3List);

                    Console.CursorLeft = 0;
                    Console.CursorTop = 13;

                    Console.WriteLine($"What do you want to do?");
                    if (!isGuest.LoggedIn)
                        Console.WriteLine($"1. Login");
                    else
                        Console.WriteLine($"1. Logout");
                    Console.WriteLine($"2. Register");
                    Console.WriteLine($"3. Profile");
                    Console.WriteLine($"4. Enter Shop");
                    Console.WriteLine($"5. Quit");
                    if (isGuest.LoggedIn && isGuest.IsAdmin)
                        Console.WriteLine($"6. Admin");
                    if (isGuest.LoggedIn)
                    {
                        Console.Write($"\nYou are currently logged in as ");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(isGuest.UserName);
                        Console.ResetColor();
                    }
                    else
                        Console.WriteLine("\nYou are currently not logged in");

                    string input = Console.ReadLine()!;

                    validInput = int.TryParse(input, out menu) && menu >= 1 && menu <= 6;

                } while (!validInput);

                db.SaveChanges();

                switch (menu)
                {
                    case 1:
                        if (!isGuest.LoggedIn)
                        {
                            Console.Clear();

                            Console.Write("Please enter Username: ");
                            string userName = Console.ReadLine()!;

                            bool foundCustomer = false;
                            await foreach (var customer in db.Customer)
                            {
                                if (customer.UserName == userName)
                                {
                                    foundCustomer = true;
                                }
                            }

                            if (foundCustomer)
                            {
                                await foreach (var customer in db.Customer)
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
                                            isGuest.LoggedIn = true;
                                            isGuest.UserName = customer.UserName;
                                            isGuest.IsAdmin = customer.IsAdmin;
                                            Thread.Sleep(1500);

                                        }
                                        else
                                        {
                                            Console.WriteLine("Incorret password!");
                                            Thread.Sleep(1500);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No username found");
                                Thread.Sleep(1500);
                            }

                            db.SaveChanges();
                            break;
                        }
                        else
                        {
                            await foreach (var customer in db.Customer)
                            {
                                customer.LoggedIn = false;
                            }
                            db.SaveChanges();
                            isGuest.LoggedIn = false;
                            isGuest.IsAdmin = false;
                            db.SaveChanges();
                            break;
                        }

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Type B to Back\n");
                        Console.Write("Please enter your Firstname: ");
                        string newFirstName = Console.ReadLine()!;
                        if (Admin.BackOption(newFirstName))
                            break;
                        Console.Write("Please enter your Lastname: ");
                        string newLastname = Console.ReadLine()!;
                        if (Admin.BackOption(newLastname))
                            break;
                        int newAge = 0;
                        while (newAge <= 0)
                        {
                            Console.Write("Please enter your Age: ");
                            string inputCheck = Console.ReadLine()!;
                            if (Admin.BackOption(inputCheck))
                                break;
                            if (int.TryParse(inputCheck, out newAge) && !string.IsNullOrWhiteSpace(inputCheck) && newAge > 0)
                            {
                            }
                            else
                                Console.WriteLine("Invalid Input");
                        }

                        string newUserName = "";
                        while (true)
                        {
                            Console.Write("Please enter UserName: ");
                            newUserName = Console.ReadLine()!;

                            if (Admin.BackOption(newUserName))
                                break;
                            var customers = db.Customer.Where(x => x.UserName == newUserName).SingleOrDefault();

                            if (customers != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("The username is already taken\n");
                                Console.ResetColor();
                                Thread.Sleep(1500);
                            }
                            else
                                break;
                        }

                        Console.Write("Please enter your Password: ");
                        string newPassword = Console.ReadLine()!;
                        if (Admin.BackOption(newPassword))
                            break;

                        db.Customer.Add(new Customer
                        (
                            newFirstName,
                            newLastname,
                            newAge,
                            newUserName,
                            newPassword,
                            false
                        ));
                        db.SaveChanges(); // måste spara användaren Innan man kan göra det som kommer under

                        foreach (var customer in db.Customer)
                        {
                            if (customer.LoggedIn)
                            {
                                customer.Logins++;
                                isGuest.LoggedIn = true;
                                isGuest.UserName = newUserName;
                                isGuest.IsAdmin = false;
                            }
                        }
                        db.SaveChanges(); // Då användaren är skapad, så  skall man automatiskt blir inloggad vid registrering så skall även inloggningsräknaren ökas

                        break;

                    //Profile
                    case 3:
                        Console.Clear();
                        var person = db.Customer.Where(x => x.UserName == isGuest.UserName).SingleOrDefault();

                        if (isGuest.LoggedIn)
                        {
                            while (true)
                            {
                                Console.Clear();
                                GUI.DrawWindow("Profile", 5, 10, new List<string>() {
                                                $"1. Firstname: {person.Name}",
                                                $"2. Lastname: {person.LastName}",
                                                $"3. Age: {person.Age}",
                                                $"4. Username: {person.UserName}",
                                                $"5. Total orders: {person.TotalOrders}",
                                                $"6. Total Logins: {person.Logins}",
                                                $"7. Registered: {person.Registered}"
                                            });

                                GUI.DrawWindow("Stuff", 45, 10, new List<string>() {
                                                $"1. To change Password",
                                                $"2. To see OrderHistory",
                                                $"3. To see total money spent",
                                                $"B to back"
                                            });
                                int updateNumber = 0;

                                Console.SetCursorPosition(5, 20);
                                string profileCheck = Console.ReadLine()!;

                                if (profileCheck.ToLower() == "b")
                                    break;

                                if (int.TryParse(profileCheck, out updateNumber) && !string.IsNullOrWhiteSpace(profileCheck) && updateNumber > 0 && updateNumber < 4)
                                {
                                    var oderHistory = db.Order.Include(x => x.Customer)
                                                    .Include(x => x.Products)
                                                    .Where(x => x.Customer.UserName == isGuest.UserName);
                                    switch (updateNumber)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("\nPress B to back");
                                            Console.WriteLine($"Current password: {person.Password}");
                                            Console.WriteLine("What do you want to change your Password to?: ");
                                            string passwordUpdate = Console.ReadLine()!;

                                            if (Admin.BackOption(passwordUpdate))
                                                break;

                                            if (!string.IsNullOrWhiteSpace(passwordUpdate))
                                            {
                                                person.Password = passwordUpdate;
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("Password Changed");
                                                Thread.Sleep(1000);
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("Invalid Input");
                                                Thread.Sleep(1000);
                                            }
                                            Console.ResetColor();
                                            db.SaveChanges();
                                            break;

                                        case 2:
                                            while (true)
                                            {
                                                Console.Clear();
                                                foreach (var item in oderHistory)
                                                {
                                                    Console.WriteLine($"OrderID: {item.Id}");
                                                    Console.WriteLine("-- Products --------------------------------------------");
                                                    foreach (var product in item.Products)
                                                    {
                                                        Console.WriteLine($"{product.Name.PadRight(48)} Amount: {product.Amount} Price/Unit: {product.Price:C}");
                                                    }
                                                    Console.WriteLine($"---- Shipping: {item.ShippingFee:C} ---- Taxes: {Convert.ToInt32((item.TotalAmountPrice - item.ShippingFee) * 0.25):C} ---- Total Price: {item.TotalAmountPrice:C} ----\n");
                                                }
                                                Console.WriteLine("B to Back");
                                                string value = Console.ReadLine()!;
                                                if (Admin.BackOption(value))
                                                    break;
                                            }

                                            break;

                                        case 3:
                                            while (true)
                                            {
                                                Console.Clear();
                                                double? moneySpent = 0;
                                                foreach (var item in oderHistory)
                                                {
                                                    moneySpent += item.TotalAmountPrice;

                                                }
                                                Console.Write($"Total money spent here: ");
                                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                                Console.WriteLine($"{moneySpent:C}");
                                                Console.ResetColor();
                                                Console.WriteLine("B to back");
                                                string backCheck = Console.ReadLine()!;

                                                if (Admin.BackOption(backCheck))
                                                    break;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Need to be logged in to see Profile");
                            Thread.Sleep(1000);
                        }
                        break;

                    case 4:

                        var topSeller = db.Shop
                            .OrderByDescending(x => x.Sold)
                            .Take(3);

                        var items = db.Shop.GroupBy(x => x.Category);

                        while (true)
                        {
                            Console.Clear();

                            if (isGuest.LoggedIn)
                            {
                                GUI.DrawWindowForCart("Shopping Cart", totalAmount, 20, 26, cartProductsInString);
                                Console.SetCursorPosition(0, 0);
                            }

                            int validNum = 0;
                            int categoryNum = 0;
                            foreach (var cat in items)
                            {
                                categoryNum++;
                                Console.WriteLine($"{categoryNum}. Category: {cat.Key}");
                                Console.WriteLine("----------------------");
                            }
                            categoryNum++;
                            Console.WriteLine($"{categoryNum}. to search for a Product by name\n");

                            Console.WriteLine("Press B to back");
                            Console.Write("Which product do you want to enter?: ");

                            string numberCheck = Console.ReadLine()!;

                            if (numberCheck.ToLower() == "o")
                            {
                                if (cartProducts.Count > 0)
                                    Order();
                            }
                            else if (numberCheck.ToLower() == "b")
                            {
                                break;
                            }
                            else if (numberCheck.ToLower() == "c")
                            {
                                foreach (var product in cartProducts)
                                {
                                    var itemToBuy = db.Shop.Where(n => n.Name == product.Name)
                                    .SingleOrDefault();
                                    itemToBuy!.Quantity += product.Amount;
                                }
                                cartProductsInString.Clear();
                                cartProducts.Clear();
                                totalAmount = 0;
                            }
                            else if (int.TryParse(numberCheck, out validNum))
                            {
                                int counter = 0;
                                string categoryName = "";
                                foreach (var cate in items)
                                {
                                    // Då jag inte börjar ifrån 0 med min text så får jag ta med -1 för att få det till rätt Index
                                    if (counter == validNum - 1)
                                        categoryName = cate.Key;

                                    counter++;
                                }

                                if (!string.IsNullOrWhiteSpace(categoryName))
                                    await GettingProducts(categoryName);

                                else if (validNum == categoryNum)
                                {
                                    while (true)
                                    {
                                        List<Product> productList = new List<Product>();
                                        Console.Clear();

                                        if (isGuest.LoggedIn)
                                        {
                                            GUI.DrawWindowForCart("Shopping Cart", totalAmount, 20, 26, cartProductsInString);
                                            Console.SetCursorPosition(0, 0);
                                        }
                                        Console.Write("Which product do you want to see?: ");
                                        string productName = Console.ReadLine()!;

                                        var gettingProductName = db.Shop.Where(x => EF.Functions.Like(x.Name, $"%{productName}%")).ToList();

                                        if (gettingProductName.Count() != 0)
                                        {
                                            foreach (var product in gettingProductName)
                                            {
                                                Console.WriteLine($"ID: {product.Id} - In stock: {product.Quantity.ToString().PadRight(2)} - {product.Name}");
                                            }
                                            Console.WriteLine();
                                            // Så man inte kan lägga i varukorgen när man inte är inloggad
                                            if (isGuest.LoggedIn)
                                                Console.WriteLine("Type ID number you want to add to Cart");

                                            Console.WriteLine("B to Back");
                                            string addToCart = Console.ReadLine()!;
                                            int intToCart = 0;

                                            if (addToCart.ToLower() == "b")
                                                break;
                                            else if (addToCart.ToLower() == "o")
                                            {
                                                if (cartProducts.Count > 0)
                                                    Order();
                                            }
                                            else if (addToCart.ToLower() == "c")
                                            {
                                                foreach (var product in cartProducts)
                                                {
                                                    var itemToBuy = db.Shop.Where(n => n.Name == product.Name)
                                                    .SingleOrDefault();
                                                    itemToBuy!.Quantity += product.Amount;
                                                }
                                                cartProductsInString.Clear();
                                                cartProducts.Clear();
                                                totalAmount = 0;

                                            }
                                            else if (int.TryParse(addToCart, out intToCart) && isGuest.LoggedIn)
                                            {
                                                var idChecker = await db.Shop.OrderBy(x => x.Id).ToListAsync();
                                                bool idCheck = false;
                                                foreach (var item in idChecker)
                                                {
                                                    if (item.Id == intToCart)
                                                        idCheck = true;
                                                }

                                                if (!idCheck)
                                                {
                                                    Console.WriteLine("Invalid Input");
                                                    Thread.Sleep(1500);
                                                }
                                                else
                                                {
                                                    Console.WriteLine("How many of these?");
                                                    string stringAmountAdd = Console.ReadLine()!.ToLower();
                                                    int intAmountAdd = 0;


                                                    if (int.TryParse(stringAmountAdd, out intAmountAdd) && !string.IsNullOrWhiteSpace(stringAmountAdd))
                                                    {
                                                        var itemToBuy = db.Shop.Where(s => s.Id == intToCart)
                                                                                .SingleOrDefault();

                                                        if (intAmountAdd <= itemToBuy.Quantity)
                                                        {
                                                            bool nameCheck = false;
                                                            if (!cartProducts.IsNullOrEmpty())
                                                            {
                                                                foreach (var item in cartProducts)
                                                                {
                                                                    if (item.Name == itemToBuy.Name)
                                                                    {
                                                                        item.Amount += intAmountAdd;
                                                                        nameCheck = true;
                                                                    }
                                                                }
                                                                if (!nameCheck)
                                                                {
                                                                    cartProducts.Add(new Product(itemToBuy.Name, intAmountAdd, itemToBuy.Price));
                                                                    productList.Add(new Product(itemToBuy.Name, intAmountAdd, itemToBuy.Price));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                cartProducts.Add(new Product(itemToBuy.Name, intAmountAdd, itemToBuy.Price));
                                                                productList.Add(new Product(itemToBuy.Name, intAmountAdd, itemToBuy.Price));
                                                            }

                                                            cartProductsInString.Clear();
                                                            totalAmount = 0;

                                                            foreach (var product in cartProducts)
                                                            {
                                                                totalAmount += Convert.ToDouble(product.Amount * product.Price);
                                                                cartProductsInString.Add($"Name: {product.Name.PadRight(51)}\t Amount: {product.Amount}, Price: {(product.Price * product.Amount)}");
                                                                if (product.Name == itemToBuy.Name)
                                                                    itemToBuy.Quantity -= product.Amount;
                                                            }
                                                            cartProductsInString.Add($"---------------------------------------------------");
                                                            cartProductsInString.Add($"Press C to cancel order or press O to enter Order");
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Cant buy more then there is in stock");
                                                            Thread.Sleep(1000);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Input");
                                                Thread.Sleep(1500);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("No product found with that search, Try again");
                                            Thread.Sleep(2000);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Must be a number from {categoryNum / categoryNum} to {categoryNum}");
                                    Console.ResetColor();
                                    Thread.Sleep(1500);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Input");
                                Thread.Sleep(1500);
                            }
                        }

                        db.SaveChanges();
                        break;

                    case 5:
                        running = false;
                        Console.WriteLine("Good bye, see you later!");
                        Thread.Sleep(1000);

                        break;

                    case 6:
                        if (isGuest.IsAdmin)
                        {
                            Console.Clear();

                            await Admin.AdminConsol(isGuest);
                        }
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
    static async Task GettingProducts(string categoryName)
    {
        int addToOrder = 0;

        while (true)
        {
            Console.Clear();
            using (var db = new MyDbContext())
            {
                List<Product> productList = new List<Product>();
                ShopItems(categoryName);


                if (!isGuest.LoggedIn)
                {
                    Console.WriteLine("\nPress B to back");
                    Console.WriteLine("Need to login to buy stuff");
                    string back = Console.ReadLine()!;
                    if (back == "b")
                        break;
                }
                else
                {
                    Console.WriteLine("\nWhich Product do you want to buy?");
                    Console.WriteLine("\nPress B to back");

                    GUI.DrawWindowForCart("Shopping Cart", totalAmount, 20, 26, cartProductsInString); // Före
                    Console.SetCursorPosition(0, 31);


                    string orderAdd = Console.ReadLine()!.ToLower();
                    if (orderAdd.ToLower() == "b")
                        break;

                    // Denna är till för att få hur många artiklar det ligger i Lager så jag har något att gå på
                    var idCounter = await db.Shop.OrderBy(x => x.Id).ToListAsync();

                    if (orderAdd.ToLower() == "c")
                    {
                        foreach (var product in cartProducts)
                        {
                            var itemToBuy = db.Shop.Where(n => n.Name == product.Name)
                            .SingleOrDefault();
                            itemToBuy!.Quantity += product.Amount;
                        }
                        cartProductsInString.Clear();
                        cartProducts.Clear();
                        totalAmount = 0;
                    }
                    else if (orderAdd.ToLower() == "o")
                    {
                        if (cartProducts.Count > 0)
                            Order();
                    }
                    else if (int.TryParse(orderAdd, out addToOrder) && addToOrder > 0 && !string.IsNullOrWhiteSpace(orderAdd))    // Här kollas antalet artiklar
                    {
                        // Här börjar kontrollen för att se om angivet nummer av användaren vilket är ID av Produkter, finns i Databasen
                        bool isTrue = false;
                        foreach (var item in idCounter)
                        {
                            if (item.Id == addToOrder)
                                isTrue = true;
                        }
                        if (!isTrue)
                        {
                            // Om den inte finns så skickas en varning ut och man fortsätter i samma ruta
                            Console.WriteLine("Product not Found!");
                            Thread.Sleep(2000);
                            continue;
                        }
                        // Här slutar det

                        Console.WriteLine("How many of these?");
                        string amountAdd = Console.ReadLine()!.ToLower();
                        int amount = 0;


                        if (int.TryParse(orderAdd, out addToOrder) && !string.IsNullOrWhiteSpace(orderAdd) && int.TryParse(amountAdd, out amount) && !string.IsNullOrWhiteSpace(amountAdd) && amount > 0)
                        {
                            var itemToBuy = db.Shop.Where(s => s.Id == addToOrder)
                                                    .SingleOrDefault();

                            if (amount <= itemToBuy.Quantity)
                            {
                                bool nameCheck = false;
                                if (!cartProducts.IsNullOrEmpty())
                                {
                                    foreach (var item in cartProducts)
                                    {
                                        if (item.Name == itemToBuy.Name)
                                        {
                                            item.Amount += amount;
                                            nameCheck = true;
                                        }
                                    }
                                    if (!nameCheck)
                                    {
                                        cartProducts.Add(new Product(itemToBuy.Name, amount, itemToBuy.Price));
                                        productList.Add(new Product(itemToBuy.Name, amount, itemToBuy.Price));
                                    }
                                }
                                else
                                {
                                    cartProducts.Add(new Product(itemToBuy.Name, amount, itemToBuy.Price));
                                    productList.Add(new Product(itemToBuy.Name, amount, itemToBuy.Price));
                                }

                                cartProductsInString.Clear();
                                totalAmount = 0;

                                foreach (var product in cartProducts)
                                {
                                    totalAmount += Convert.ToDouble(product.Amount * product.Price);
                                    cartProductsInString.Add($"Name: {product.Name.PadRight(51)}\t Amount: {product.Amount}, Price: {(product.Price * product.Amount)}");
                                    if (product.Name == itemToBuy.Name)
                                        itemToBuy.Quantity -= product.Amount;
                                }
                                cartProductsInString.Add($"---------------------------------------------------");
                                cartProductsInString.Add($"Press C to cancel order or press O to enter Order");
                            }
                            else
                            {
                                Console.WriteLine("Cant buy more then there is in stock");
                                Thread.Sleep(1000);
                            }
                        }

                        GUI.DrawWindowForCart("Shopping Cart", totalAmount, 20, 26, cartProductsInString); // Efter

                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                        Thread.Sleep(1000);
                    }
                }
                db.SaveChanges();
            }
        }
    }
    static void ShopItems(string categoryName)
    {
        using (var db = new MyDbContext())
        {
            var itemsSubcategory = db.Shop.Where(cn => cn.Category == categoryName)
                                                .OrderBy(cn => cn.Id)
                                                .ThenBy(cn => cn.SubCategory)
                                                .GroupBy(sc => sc.SubCategory);

            Console.Clear();

            // Denna loopen skriver ut listan så man ser vad man kan köpa
            foreach (var sub in itemsSubcategory)
            {

                Console.WriteLine($"Category: {sub.Key}\n----------------------");
                foreach (var item in sub)
                {
                    Console.Write($"{item.Id}. Name: {item.Name} Total in stock: ");

                    if (item.Quantity > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{item.Quantity}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{item.Quantity}");
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
        }
    }
    static void Order()
    {
        while (true)
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();
                int orderCheckNumber = 0;
                string nameCheck = "";


                double? totalprice = 0;

                Console.WriteLine("-- Cart --------------------------");
                for (int i = 0; i < cartProducts.Count; i++)
                {
                    Console.WriteLine($"Product Name: {cartProducts[i].Name.PadRight(48)}\t Amount: {cartProducts[i].Amount}\t Price: {cartProducts[i].Price:C}");
                    totalprice += (cartProducts[i].Price * cartProducts[i].Amount);
                }
                if (cartProducts.Count == 0)
                    Console.WriteLine("Cart is empty");
                else
                {
                    Console.Write($"\nTotal Price: ");

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"{totalprice:C}");
                    Console.ResetColor();

                    Console.Write(" which ");

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"{totalprice * 0.25:C}");
                    Console.ResetColor();

                    Console.WriteLine(" is Tax");
                }
                Console.WriteLine("----------------------------------\n");

                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. To Change the amount of Product in cart");
                Console.WriteLine("2. To Delete a Product from cart");
                Console.WriteLine("3. Buy items");
                Console.WriteLine("4. Abort Order");
                Console.WriteLine("B to Back");
                string orderCheck = Console.ReadLine()!;

                if (Admin.BackOption(orderCheck))
                    break;

                if (int.TryParse(orderCheck, out orderCheckNumber) && !string.IsNullOrWhiteSpace(orderCheck) && orderCheckNumber > 0 && orderCheckNumber < 5)
                {
                    switch (orderCheckNumber)
                    {
                        // Update amount of products
                        case 1:
                            int numberInList = 0;
                            while (true)
                            {
                                Console.Clear();

                                Console.WriteLine("Which product do you want to alter the amount of:");
                                Console.WriteLine("--------------------------");
                                for (int i = 0; i < cartProducts.Count; i++)
                                {
                                    Console.WriteLine($"Id.{i + 1} {cartProducts[i].Name.PadRight(48)} Amount: {cartProducts[i].Amount}");
                                }
                                Console.WriteLine("--------------------------\n");
                                Console.WriteLine("B to Back");
                                string productCheck = Console.ReadLine()!;

                                if (productCheck.ToLower() == "b")
                                    break;

                                if (int.TryParse(productCheck, out numberInList) && !string.IsNullOrWhiteSpace(productCheck) && numberInList > 0 && numberInList <= cartProducts.Count)
                                {
                                    for (int i = 0; i < cartProducts.Count; i++)
                                    {
                                        if (i == numberInList - 1)
                                            nameCheck = cartProducts[i].Name;
                                    }
                                    while (true)
                                    {

                                        int numberToIncDec = 0;
                                        Console.Clear();

                                        var singleProduct = db.Shop.Where(x => x.Name == nameCheck).SingleOrDefault();
                                        foreach (var item in cartProducts)
                                        {
                                            if (item.Name.Contains(singleProduct.Name))
                                            {
                                                Console.Write("Amount: ");
                                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                Console.Write($"{item.Amount}");
                                                Console.ResetColor();
                                                Console.Write($" - Left in stock: ");
                                                if (singleProduct.Quantity != 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                    Console.Write($"{singleProduct.Quantity}");
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write($"{singleProduct.Quantity}");
                                                }
                                                Console.ResetColor();
                                                Console.WriteLine($"\tName: {item.Name}");
                                            }
                                        }
                                        Console.WriteLine("1. To Increase amount of Product");
                                        Console.WriteLine("2. To Decrease amount of Product");
                                        Console.WriteLine("B to back");
                                        string stringIncDec = Console.ReadLine()!;

                                        if (stringIncDec.ToLower() == "b")
                                            break;

                                        if (int.TryParse(stringIncDec, out numberToIncDec) && !string.IsNullOrWhiteSpace(stringIncDec) && numberToIncDec > 0 && numberToIncDec < 3)
                                        {
                                            bool isTrue = true;
                                            switch (numberToIncDec)
                                            {
                                                case 1:
                                                    foreach (var item in cartProducts)
                                                    {
                                                        if (item.Name.Contains(singleProduct.Name) && 0 < singleProduct.Quantity)
                                                        {
                                                            item.Amount++;
                                                            singleProduct.Quantity--;
                                                            isTrue = false;
                                                        }

                                                    }
                                                    if (isTrue)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("No more Product in stock to add");
                                                        Thread.Sleep(1000);
                                                        Console.ResetColor();
                                                    }
                                                    break;

                                                case 2:
                                                    foreach (var item in cartProducts)
                                                    {
                                                        if (item.Name.Contains(singleProduct.Name) && item.Amount > 0)
                                                        {
                                                            item.Amount--;
                                                            singleProduct.Quantity++;
                                                            isTrue = false;
                                                        }
                                                    }
                                                    if (isTrue)
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("Cant be less then 0");
                                                        Thread.Sleep(1000);
                                                        Console.ResetColor();
                                                    }
                                                    break;
                                            }

                                            cartProductsInString.Clear();
                                            totalAmount = 0;

                                            foreach (var product in cartProducts)
                                            {
                                                totalAmount += Convert.ToDouble(product.Amount * product.Price);
                                                cartProductsInString.Add(new string($"Name: {product.Name.PadRight(51)}\t Amount: {product.Amount}, Price: {(product.Price * product.Amount)}"));
                                            }
                                            cartProductsInString.Add($"---------------------------------------------------");
                                            cartProductsInString.Add($"Press C to cancel order or press O to enter Order");

                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input");
                                    Thread.Sleep(1000);
                                }
                            }
                            db.SaveChanges();
                            break;

                        // Delete Product in Cart
                        case 2:
                            while (true)
                            {
                                Console.Clear();
                                int numberToDelete = 0;

                                Console.WriteLine("Which product do you want to delete:");
                                Console.WriteLine("--------------------------");
                                for (int i = 0; i < cartProducts.Count; i++)
                                {
                                    Console.WriteLine($"Id.{i + 1} {cartProducts[i].Name.PadRight(48)} Amount: {cartProducts[i].Amount}");
                                }
                                Console.WriteLine("--------------------------\n");
                                Console.WriteLine("B to Back");
                                string productToDelete = Console.ReadLine()!;

                                if (productToDelete.ToLower() == "b")
                                    break;

                                if (int.TryParse(productToDelete, out numberToDelete) && !string.IsNullOrWhiteSpace(productToDelete) && numberToDelete > 0 && numberToDelete <= cartProducts.Count)
                                {
                                    string productNameCheck = "";
                                    bool productDelete = false;
                                    for (int i = 0; i < cartProducts.Count; i++)
                                    {
                                        if (i == numberToDelete - 1)
                                        {
                                            productNameCheck = cartProducts[i].Name;
                                            productDelete = true;
                                            totalprice -= cartProducts[i].Price * cartProducts[i].Amount;
                                        }
                                    }
                                    var singleProduct = db.Shop.Where(x => x.Name == productNameCheck).SingleOrDefault();

                                    if (productDelete)
                                    {
                                        singleProduct.Quantity += cartProducts[numberToDelete - 1].Amount;
                                        cartProducts.RemoveAt(numberToDelete - 1);
                                        if (cartProducts.Count() == 0)
                                        {
                                            totalAmount = 0;
                                            cartProductsInString.Clear();
                                        }
                                        else
                                            cartProductsInString.RemoveAt(numberToDelete - 1);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input");
                                    Thread.Sleep(1000);
                                }

                            }
                            db.SaveChanges();
                            break;
                        // Buy all stuff in cart
                        case 3:
                            Console.Clear();

                            var order = db.Order.ToList();
                            var person = db.Customer.Where(x => x.UserName == isGuest.UserName).SingleOrDefault();

                            while (true)
                            {
                                int? amountOfProducts = 0;
                                double? totalPriceCheckout = 0;

                                Console.Clear();
                                Console.WriteLine("You are about to buy these Products");
                                Console.WriteLine("-----------------------------------------------------------");
                                foreach (var item in cartProducts)
                                {
                                    amountOfProducts += item.Amount;
                                    totalPriceCheckout += item.Amount * item.Price;
                                    Console.WriteLine($"{item.Name.PadRight(48)} Amount: {item.Amount} - Price/Unit: {item.Price:C} - Price*Amount: {item.Price * item.Amount:C}");
                                }
                                Console.Write($"Total Price: ");
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine($"{totalPriceCheckout:C}");
                                Console.ResetColor();
                                Console.WriteLine("-----------------------------------------------------------\n");

                                Console.WriteLine($"1. to Buy \nB to back"); //Oneliner
                                string checkOutCheck = Console.ReadLine()!;
                                int numberCheck = 0;
                                if (checkOutCheck.ToLower() == "b")
                                    break;

                                if (int.TryParse(checkOutCheck, out numberCheck) && !string.IsNullOrWhiteSpace(checkOutCheck) && numberCheck > 0 && numberCheck < 2)
                                {
                                    while (true)
                                    {
                                        Console.Clear();

                                        string orderCollection = "";
                                        Console.Clear();
                                        Console.WriteLine("Please chose one option below");
                                        Console.WriteLine("1. Collect in Store");
                                        Console.WriteLine("2. Shipping to Adress (Inc Shipping fee)");
                                        Console.WriteLine("B to back");
                                        string stringOption = Console.ReadLine()!;
                                        int intOption = 0;

                                        if (Admin.BackOption(stringOption))
                                            break;

                                        if (int.TryParse(stringOption, out intOption) && !string.IsNullOrWhiteSpace(stringOption) && intOption > 0 && intOption < 3)
                                        {
                                            int customerId = 0;
                                            foreach (var customer in db.Customer)
                                            {
                                                if (isGuest.UserName == customer.UserName)
                                                    customerId = customer.Id;
                                            }
                                            Console.Clear();
                                            string payOption = "";
                                            switch (intOption)
                                            {
                                                case 1:
                                                    orderCollection = "Collect in Store";
                                                    Console.WriteLine("Option 'Collect in Store' is chosen");
                                                    Console.WriteLine("1. Pay in Store");
                                                    Console.WriteLine("2. Pay with Swish");
                                                    Console.WriteLine("B to Back");
                                                    string stringPayOptionStore = Console.ReadLine()!;
                                                    int intPayOptionStore = 0;
                                                    if (Admin.BackOption(stringPayOptionStore))
                                                        break;

                                                    if (int.TryParse(stringPayOptionStore, out intPayOptionStore) && !string.IsNullOrWhiteSpace(stringPayOptionStore) && intPayOptionStore > 0 && intPayOptionStore < 3)
                                                    {
                                                        while (true)
                                                        {
                                                            Console.Clear();
                                                            if (intPayOptionStore == 1)
                                                            {
                                                                Console.WriteLine("Option 'Pay in Store' is Chosen");
                                                                payOption = "Pay in Store";
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Option 'Pay with Swish' is Chosen");
                                                                payOption = "Pay with Swish";
                                                            }

                                                            Console.WriteLine("-----------------------------");
                                                            Console.WriteLine("B to back\n");
                                                            Console.Write("Please enter Firstname followed by Lastname: ");
                                                            string name = Console.ReadLine()!;
                                                            if (Admin.BackOption(name))
                                                                break;

                                                            if (!string.IsNullOrWhiteSpace(name))
                                                            {
                                                                db.Order.Add(new Order()
                                                                {
                                                                    CustomerId = customerId,
                                                                    TotalItems = amountOfProducts,
                                                                    PaymentChoice = payOption,
                                                                    Shipping = orderCollection,
                                                                    TotalAmountPrice = totalPriceCheckout,
                                                                    Products = cartProducts
                                                                });

                                                                Console.ForegroundColor = ConsoleColor.Green;

                                                                db.SaveChanges();

                                                                var products = db.Shop.ToList();

                                                                foreach (var item in cartProducts)
                                                                {
                                                                    foreach (var prod in products)
                                                                    {
                                                                        if (item.Name == prod.Name)
                                                                        {
                                                                            prod.Sold += item.Amount;
                                                                            db.SaveChanges();
                                                                        }
                                                                    }
                                                                }

                                                                person.TotalOrders++;
                                                                db.SaveChanges();

                                                                Console.WriteLine("Sucess on buying Order");
                                                                Console.ResetColor();
                                                                Thread.Sleep(1000);

                                                                cartProducts.Clear();
                                                                cartProductsInString.Clear();
                                                                totalAmount = 0;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Invalid Input");
                                                        Thread.Sleep(1000);
                                                    }
                                                    db.SaveChanges();
                                                    break;

                                                case 2:
                                                    var regularShipping = db.Shop.Select(x => x.RegularShipping).First();
                                                    var expressShipping = db.Shop.Select(x => x.ExpressShipping).First();

                                                    orderCollection = "Shipping to Adress";
                                                    int? fee = 0;
                                                    Console.WriteLine("Option 'Shipping to Adress' is chosen");
                                                    Console.WriteLine($"1. Regular shipping (3-4 days) - Shipping fee = {regularShipping}");
                                                    Console.WriteLine($"2. Express shipping (1-2 days) - Shipping fee = {expressShipping}");
                                                    Console.WriteLine("B to Back");
                                                    string shippingOption = Console.ReadLine()!;

                                                    if (shippingOption == "b")
                                                        break;
                                                    else if (shippingOption == "1")
                                                        fee = regularShipping;
                                                    else if (shippingOption == "2")
                                                        fee = expressShipping;
                                                    else
                                                    {
                                                        Console.WriteLine("Invalid Input");
                                                        Thread.Sleep(1500);
                                                        break;
                                                    }
                                                    Console.WriteLine($"Total price inc fee and tax = {totalAmount + fee:C}");
                                                    Console.WriteLine("\n1. Pay with Creditcard");
                                                    Console.WriteLine("2. Pay with Swish");
                                                    Console.WriteLine("B to Back");
                                                    string stringPayOptionShipping = Console.ReadLine()!;
                                                    int intPayOptionShipping = 0;
                                                    if (Admin.BackOption(stringPayOptionShipping))
                                                        break;

                                                    if (int.TryParse(stringPayOptionShipping, out intPayOptionShipping) && !string.IsNullOrWhiteSpace(stringPayOptionShipping) && intPayOptionShipping > 0 && intPayOptionShipping < 3)
                                                    {
                                                        while (true)
                                                        {
                                                            Console.Clear();


                                                            if (intPayOptionShipping == 1)
                                                            {
                                                                Console.WriteLine("Option 'Pay with Creditcard' is Chosen");
                                                                payOption = "Pay with Creditcard";
                                                            }
                                                            else if (intPayOptionShipping == 2)
                                                            {
                                                                Console.WriteLine("Option 'Pay with Swish' is Chosen");
                                                                payOption = "Pay with Swish";
                                                            }

                                                            Console.WriteLine("-----------------------------");
                                                            Console.WriteLine("B to back\n");
                                                            Console.Write("Please enter Shipping adress: ");
                                                            string adress = Console.ReadLine()!;
                                                            if (Admin.BackOption(adress))
                                                                break;

                                                            Console.Write("\nPlease enter City: ");
                                                            string city = Console.ReadLine()!;
                                                            if (Admin.BackOption(city))
                                                                break;

                                                            Console.Write("\nPlease enter ZipCode: ");
                                                            string stringZipcode = Console.ReadLine()!;
                                                            int intZipcode = 0;
                                                            if (Admin.BackOption(stringZipcode))
                                                                break;
                                                            if (int.TryParse(stringZipcode, out intZipcode) && !string.IsNullOrWhiteSpace(stringZipcode) && intZipcode >= 0)
                                                            {
                                                                db.Order.Add(new Order()
                                                                {
                                                                    CustomerId = customerId,
                                                                    TotalItems = amountOfProducts,
                                                                    PaymentChoice = payOption,
                                                                    Shipping = orderCollection,
                                                                    ShippingFee = 500,
                                                                    TotalAmountPrice = totalPriceCheckout + 500,
                                                                    Products = cartProducts,
                                                                    City = city,
                                                                    Adress = adress,
                                                                    Zipcode = intZipcode
                                                                });
                                                                Console.ForegroundColor = ConsoleColor.Green;

                                                                db.SaveChanges();

                                                                var products = db.Shop.ToList();

                                                                foreach (var item in cartProducts)
                                                                {
                                                                    foreach (var prod in products)
                                                                    {
                                                                        if (item.Name == prod.Name)
                                                                        {
                                                                            prod.Sold += item.Amount;
                                                                            db.SaveChanges();
                                                                        }
                                                                    }
                                                                }

                                                                person.TotalOrders++;
                                                                db.SaveChanges();

                                                                Console.WriteLine("Sucess on buying Order");
                                                                Console.ResetColor();
                                                                Thread.Sleep(1000);

                                                                cartProducts.Clear();
                                                                cartProductsInString.Clear();
                                                                totalAmount = 0;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Invalid Input");
                                                        Thread.Sleep(1000);
                                                    }
                                                    db.SaveChanges();
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input");
                                    Thread.Sleep(1000);
                                }
                            }
                            db.SaveChanges();
                            break;

                        // Abort Order
                        case 4:
                            foreach (var product in cartProducts)
                            {
                                var itemToBuy = db.Shop.Where(n => n.Name == product.Name)
                                .SingleOrDefault();
                                itemToBuy!.Quantity += product.Amount;
                            }
                            cartProductsInString.Clear();
                            cartProducts.Clear();
                            totalAmount = 0;

                            db.SaveChanges();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                    Thread.Sleep(1000);
                }

                db.SaveChanges();
            }
        }
    }
}
