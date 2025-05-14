using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class RunProgram
{
    static string categoryName = ""; // Sätter denna här så att jag kan komma åt den i mina metoder       
    static List<Product> cartProducts = new List<Product>();
    static List<string> cartProductsInString = new List<string>();
    static string loggedinName = "";
    static double totalAmount = 0;
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

                    await foreach (var customer in db.Customer)
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
                    if (!loggedinName.IsNullOrEmpty())
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

                            bool foundCustomer = false;
                            await foreach (var customer in db.Customer)
                            {

                                if (customer.UserName == userName)
                                {
                                    foundCustomer = true;
                                }

                            }
                            ;

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
                            loggedinName = "";
                            db.SaveChanges();
                            break;
                        }

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Type B to Back\n");
                        Console.Write("Please enter ur Firstname: ");
                        string newFirstName = Console.ReadLine()!;
                        if (Admin.BackOption(newFirstName))
                            break;
                        Console.Write("Please enter ur Lastname: ");
                        string newLastname = Console.ReadLine()!;
                        if (Admin.BackOption(newLastname))
                            break;
                        int newAge = 0;
                        while (newAge <= 0)
                        {
                            Console.Write("Please enter ur Age: ");
                            string inputCheck = Console.ReadLine()!;
                            if (Admin.BackOption(inputCheck))
                                break;
                            if (int.TryParse(inputCheck, out newAge) && !string.IsNullOrWhiteSpace(inputCheck) && newAge < 0)
                                Console.WriteLine("Must be numbers");

                        }

                        Console.Write("Please enter UserName: ");
                        string newUserName = Console.ReadLine()!;
                        if (Admin.BackOption(newUserName))
                            break;
                        Console.Write("Please enter ur Password: ");
                        string newPassword = Console.ReadLine()!;
                        if (Admin.BackOption(newPassword))
                            break;

                        db.Customer.Add(new Customer
                        (
                            newFirstName,
                            newLastname,
                            newAge,
                            newUserName,
                            newPassword
                        ));
                        db.SaveChanges(); // måste spara användaren Innan man kan göra det som kommer under

                        foreach (var customer in db.Customer)
                        {
                            if (customer.LoggedIn)
                                customer.Logins++;
                        }
                        db.SaveChanges(); // Då det är skapat så man automatiskt blir inloggad vid registrering så skall den även spara inloggnings räknaren

                        break;

                    case 3:
                        Console.Clear();

                        await foreach (var customer in db.Customer)
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
                                                $"5. Total orders: {customer.OrderHistory.Count()}",
                                                $"6. Total Logins: {customer.Logins}"
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
                        if (loggedinName == "")
                        {
                            Console.WriteLine("Need to be logged in to see Profile");
                            Thread.Sleep(1000);
                        }
                        break;

                    case 4:
                        Console.Clear();

                        var topSeller = db.Shop
                            .OrderByDescending(x => x.Sold)
                            .Take(3);

                        var items = db.Shop.GroupBy(x => x.Category);

                        bool customerLogedIn = false;
                        await foreach (var customer in db.Customer)
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
                                    GUI.DrawWindowForCart("Shopping Cart", totalAmount, 20, 26, cartProductsInString);
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
                                await GettingProducts();
                            }
                        }

                        db.SaveChanges();
                        break;

                    case 5:
                        Console.Clear();

                        await Admin.AdminConsol();

                        break;

                    case 6:
                        running = false;
                        Console.WriteLine("Good bye, see you later!");
                        Thread.Sleep(1000);
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

    static async Task GettingProducts()
    {
        int addToOrder = 0;

        while (true)
        {
            using (var db = new MyDbContext())
            {
                List<Product> productList = new List<Product>();
                ShopItems();


                if (loggedinName == "")
                {
                    Console.WriteLine("\nPress B to back");
                    Console.WriteLine("Need to login to buy stuff");
                    string back = Console.ReadLine()!;
                    if (back == "b")
                        break;
                }
                else
                {
                    Console.WriteLine("\nWich Product do you want to buy?");
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
                        Order();
                    }
                    else if (int.TryParse(orderAdd, out addToOrder) && addToOrder > 0 && addToOrder <= idCounter.Count())    // Här kollas antalet artiklar
                    {
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
                                    cartProductsInString.Add(new string($"Name: {product.Name.PadRight(51)}\t Amount: {product.Amount}, Price: {(product.Price * product.Amount)}"));
                                    if (product.Name == itemToBuy.Name)
                                        itemToBuy.Quantity -= product.Amount;
                                }
                                cartProductsInString.Add($"---------------------------------------------------");
                                cartProductsInString.Add($"Press C to cancel order or press O to enter Order");
                                //itemToBuy.Quantity -= amount;
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

    static async Task ShopItems()
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

                    Console.Write(" wich ");

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

                                Console.WriteLine("Wich product do you want to alter the amount of:");
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
                                                        if (item.Name.Contains(singleProduct.Name) && item.Amount - 1 <= singleProduct.Quantity)
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

                                Console.WriteLine("Wich product do you want to delete:");
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
                                        }
                                    }
                                    var singleProduct = db.Shop.Where(x => x.Name == productNameCheck).SingleOrDefault();

                                    if (productDelete)
                                    {
                                        singleProduct.Quantity += cartProducts[numberToDelete - 1].Amount;
                                        cartProducts.RemoveAt(numberToDelete - 1);
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
