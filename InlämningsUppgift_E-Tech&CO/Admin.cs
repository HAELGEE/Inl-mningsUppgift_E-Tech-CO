using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class Admin
{
    public static async Task AdminConsol()
    {
        using (var db = new MyDbContext())
        {
            Console.Clear();

            bool validInput = false;
            int userInput = 0;
            // Testade göra en do while-loop här för att se vilken som blev bäst att använda
            do
            {
                Console.Clear();

                Console.WriteLine($"What do you want to do?");
                Console.WriteLine($"1.  Add Item to shop");
                Console.WriteLine($"2.  Remove Item in shop");
                Console.WriteLine($"3.  Increase/Decrease stock for items");
                Console.WriteLine($"4.  Change price for item");
                Console.WriteLine($"5.  Change Category/subcategory");
                Console.WriteLine($"6.  Change Name on Product");
                Console.WriteLine($"7.  Product info");
                Console.WriteLine($"8.  All customers & Change Customer");
                Console.WriteLine($"9.  Look Orderhistories");                    // Inte riktigt klar, får det inte utskrivet (finns inga ordrar än)
                Console.WriteLine($"10. Change top3 in menu");
                Console.WriteLine($"11. Free Search");
                Console.WriteLine($"B to Back");
                string input = Console.ReadLine()!;

                if (BackOption(input))
                    break;

                if (int.TryParse(input, out userInput) && userInput >= 1 && userInput <= 11)
                    validInput = true;
                else
                {
                    Console.WriteLine("Invalid Input");
                    Thread.Sleep(1000);
                }

            } while (!validInput);

            Console.Clear();

            var categorySearch = await db.Shop.OrderBy(i => i.Id)
                                        .GroupBy(c => new { c.Category, c.SubCategory })
                                        .ToListAsync();

            if (userInput > 0 && userInput < 7)
            {
                foreach (var cat in categorySearch)
                {
                    Console.WriteLine($"Category: {cat.Key.Category}");
                    Console.WriteLine($"  SubCategory: {cat.Key.SubCategory}");
                    Console.WriteLine("-----------------------");
                    foreach (var item in cat)
                    {
                        Console.WriteLine($"ID:{item.Id} \t Name: {item.Name}\t in Stock: {item.Quantity}, Price: {item.Price}");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }



            switch (userInput)
            {
                case 1:
                    while (true)
                    {
                        Console.WriteLine("Press [B] to back");
                        Console.Write("Wich Category do you want to add this item to?: ");
                        string category = Console.ReadLine()!;
                        if (category.ToLower() == "b")
                            break;

                        Console.Write("Wich Subcategory/product maker do you want to add this item to?: ");
                        string subCategory = Console.ReadLine()!;
                        if (subCategory.ToLower() == "b")
                            break;

                        Console.Write("What is the name of the product?: ");
                        string productName = Console.ReadLine()!;
                        if (productName.ToLower() == "b")
                            break;

                        string productPriceString = "";
                        double productPrice = 0;
                        while (true)
                        {
                            Console.Write("Price on Product?: ");
                            productPriceString = Console.ReadLine()!;

                            if (double.TryParse(productPriceString, out productPrice) && !string.IsNullOrWhiteSpace(productPriceString) && productPrice > 0)
                            {
                                break;
                            }
                            else if (productPriceString.ToLower() == "b")
                                break;
                            else
                            {
                                Console.WriteLine("Invalid Input");
                                Thread.Sleep(1000);
                            }

                        }
                        if (productPriceString.ToLower() == "b")
                            break;

                        string stockString = "";
                        int stock = 0;
                        while (true)
                        {
                            Console.Write("How many in stock?: ");
                            stockString = Console.ReadLine()!;

                            if (int.TryParse(stockString, out stock) && !string.IsNullOrWhiteSpace(stockString) && stock > 0)
                            {
                                break;
                            }
                            else if (stockString.ToLower() == "b")
                                break;
                            else
                            {
                                Console.WriteLine("Invalid Input");
                                Thread.Sleep(1000);
                            }
                        }
                        if (stockString.ToLower() == "b")
                            break;

                        Console.Write("Enter information about the product: ");
                        string information = Console.ReadLine()!;
                        if (information.ToLower() == "b")
                            break;



                        db.Shop.Add(new Shop
                        {
                            Category = category,
                            SubCategory = subCategory,
                            Name = productName,
                            Price = productPrice,
                            Quantity = stock,
                            ProductInformation = information
                        });
                    }
                    db.SaveChanges();
                    break;

                case 2:
                    int deleteId = 0;
                    while (deleteId <= 0)
                    {
                        Console.Write("Wich product do you want to delete? or [B]ack: ");
                        string deleteCheck = Console.ReadLine()!;

                        if (BackOption(deleteCheck))
                            break;

                        if (int.TryParse(deleteCheck, out deleteId) && deleteId > 0 && !string.IsNullOrWhiteSpace(deleteCheck))
                        {
                            var deleteItem = db.Shop.Where(x => x.Id == deleteId);
                        }
                    }

                    db.SaveChanges();
                    break;

                case 3:
                    int updateStock = 0;
                    while (updateStock == 0)
                    {
                        Console.Write("Wich product do u want to alter the stock? or [B]ack: ");
                        string updateCheck = Console.ReadLine()!;

                        if (BackOption(updateCheck))
                            break;

                        if (int.TryParse(updateCheck, out updateStock) && updateStock > 0 && !string.IsNullOrWhiteSpace(updateCheck))
                        {
                            var updateItem = db.Shop.Where(x => x.Id == updateStock).SingleOrDefault();
                            updateStock = 0;
                            while (updateStock == 0)
                            {
                                Console.Write($"How much do you want to alter?: ");
                                string alterCheck = Console.ReadLine()!;
                                if (int.TryParse(alterCheck, out updateStock) && updateStock != 0)
                                {
                                    if (updateItem.Quantity >= 0)
                                        updateItem.Quantity = updateItem.Quantity + updateStock;
                                    else
                                        Console.WriteLine("You cant have negative in your balance");
                                }
                            }
                        }
                    }

                    db.SaveChanges();
                    break;
                case 4:
                    int updatePrice = 0;
                    while (updatePrice <= 0)
                    {
                        Console.Write("Wich product do u want to change price on? or [B]ack: ");
                        string priceCheck = Console.ReadLine()!;

                        if (BackOption(priceCheck))
                            break;

                        var updateItem = db.Shop.Where(x => x.Id == updatePrice).SingleOrDefault();
                        if (int.TryParse(priceCheck, out updatePrice) && updatePrice > 0 && !string.IsNullOrWhiteSpace(priceCheck))
                        {
                            if (updateItem.Price > 0)
                                updateItem.Price = updateItem.Price + updatePrice;
                            else
                                Console.WriteLine("Cant be negative in price");
                        }
                    }
                    db.SaveChanges();
                    break;

                case 5:
                    int updateCategory = 0;
                    while (updateCategory <= 0)
                    {
                        Console.Write($"Wich Product do you want to change category/subcategory on? or [B]ack: ");
                        string catSubCheck = Console.ReadLine()!;

                        if (BackOption(catSubCheck))
                            break;

                        if (int.TryParse(catSubCheck, out updateCategory) && updateCategory > 0 && !string.IsNullOrWhiteSpace(catSubCheck))
                        {
                            var categoryAndSub = db.Shop.Where(x => x.Id == updateCategory).SingleOrDefault();

                            Console.Write($"Wich Category do u want to change to?: ");
                            string categoryChange = Console.ReadLine()!;
                            Console.Write($"Wich Subcategory do u want to change to?: ");
                            string subCategoryChange = Console.ReadLine()!;

                            if (categoryChange != "" && subCategoryChange != "")
                            {
                                categoryAndSub.Category = categoryChange;
                                categoryAndSub.SubCategory = subCategoryChange;
                            }
                        }
                    }
                    db.SaveChanges();
                    break;

                case 6:
                    int updateProductName = 0;
                    while (updateProductName <= 0)
                    {
                        Console.Write($"Wich Product do you want to change Name on? or [B]ack: ");
                        string propductNameCheck = Console.ReadLine()!;

                        if (BackOption(propductNameCheck))
                            break;


                        if (int.TryParse(propductNameCheck, out updateProductName) && updateProductName > 0 && !string.IsNullOrWhiteSpace(propductNameCheck))
                        {
                            var productNameUpdate = db.Shop.Where(x => x.Id == updateProductName).SingleOrDefault();
                            Console.Write("What do you want to update the name to?: ");
                            string productNameInfo = Console.ReadLine()!;
                            productNameUpdate.Name = productNameInfo;
                        }
                    }

                    db.SaveChanges();
                    break;

                case 7:
                    int updateProductInformation = 0;
                    while (updateProductInformation <= 0)
                    {


                        Console.Write($"Wich product do you want to alter the information about? or [B]ack: ");
                        string productAlter = Console.ReadLine()!;

                        if (BackOption(productAlter))
                            break;

                        if (int.TryParse(productAlter, out updateProductInformation) && updateProductInformation > 0 && !string.IsNullOrWhiteSpace(productAlter))
                        {
                            Console.Clear();

                            var productInfo = db.Shop.Where(x => x.Id == updateProductInformation).SingleOrDefault();
                            Console.WriteLine($"Product: {productInfo.Name}");
                            Console.Write($"Information about the product: ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(productInfo.ProductInformation + "\n");
                            Console.ResetColor();
                            Console.WriteLine("What do tou want to update the information to? or [B]ack: ");
                            string checkProductInfo = Console.ReadLine()!;

                            if (BackOption(checkProductInfo))
                                break;

                            productInfo.ProductInformation = checkProductInfo;
                        }
                    }
                    db.SaveChanges();
                    break;
                case 8:
                    Console.Clear();
                    int updateCustomerInformation = 0;
                    while (updateCustomerInformation <= 0)
                    {
                        var allCustomers = db.Customer.OrderBy(x => x.Id);

                        if (allCustomers.Count() == 0)
                            Console.WriteLine("No users found");
                        else
                        {
                            foreach (var customer in allCustomers)
                            {
                                Console.WriteLine($"ID. {customer.Id} Name: {customer.Name}");
                            }
                        }
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("\nB to back");
                        Console.WriteLine("Wich Person do you want to update/delete");
                        string personString = Console.ReadLine()!;
                        if (BackOption(personString))
                            break;

                        if (int.TryParse(personString, out updateCustomerInformation) && updateCustomerInformation > -1 && !string.IsNullOrWhiteSpace(personString))
                        {
                            var customer = db.Customer.Where(x => x.Id == updateCustomerInformation).FirstOrDefault();

                            bool idCheck = false;
                            foreach (var person in allCustomers)
                            {
                                if (updateCustomerInformation == person.Id)
                                    idCheck = true;
                            }

                            if (!idCheck)
                            {
                                Console.WriteLine("No user found with that ID");
                                Thread.Sleep(1000);
                                break;
                            }
                            string updateCustomerString = "";
                            while (updateCustomerString.ToLower() != "b")
                            {
                                Console.Clear();
                                Console.Write("What do you want to do with ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Customer:");
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine($"Name: {customer.Name}   Lastname: {customer.LastName}   Age: {customer.Age}   Username: {customer.UserName}   Password: {customer.Password}   isAdmin: {customer.IsAdmin}   Logins: {customer.Logins}\n");
                                Console.ResetColor();
                                Console.WriteLine("1. Delete Customer");
                                Console.WriteLine("2. Update Customer Name");
                                Console.WriteLine("3. Update Customer Lastname");
                                Console.WriteLine("4. Update Customer Age");
                                Console.WriteLine("5. Update Customer Username");
                                Console.WriteLine("6. Update Customer Password");
                                Console.WriteLine("7. Update Customer Admin");
                                Console.WriteLine("B to back");
                                updateCustomerString = Console.ReadLine()!;
                                if (BackOption(updateCustomerString))
                                    break;


                                int number = 0;
                                if (int.TryParse(updateCustomerString, out number) && !string.IsNullOrWhiteSpace(updateCustomerString) && number > 0)
                                {
                                    switch (number)
                                    {
                                        case 1:
                                            Console.Clear();
                                            if (RunProgram.loggedinName == customer.UserName)
                                            {
                                                Console.WriteLine("Cant delete your own account");
                                                Thread.Sleep(1500);
                                                break;
                                            }
                                            Console.WriteLine($"Do you still want to delete customer {customer.Name} - {customer.LastName} ?");
                                            Console.WriteLine("Press Y for Yes or press anykey to back");
                                            string inputCheck = Console.ReadLine()!;
                                            if (inputCheck.ToLower() == "y")
                                            {
                                                var deleteCustomer = db.Customer.Where(x => x.Id == updateCustomerInformation)
                                                                                .ExecuteDelete();

                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("\nCustomer is now deleted from Database");
                                                Console.ResetColor();
                                                Thread.Sleep(1000);
                                                db.SaveChanges();
                                            }

                                            break;

                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine($"Current Customer Name: {customer.Name}");
                                            Console.WriteLine("What do you want to update the Customer Name to?");
                                            string nameUpdate = Console.ReadLine()!;
                                            customer.Name = nameUpdate;

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Name updated");
                                            Console.ResetColor();
                                            Thread.Sleep(1000);
                                            db.SaveChanges();
                                            break;

                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine($"Current Customer lastname: {customer.LastName}");
                                            Console.WriteLine("What do you want to update the Customer Lastname to?");
                                            string lastnameUpdate = Console.ReadLine()!;
                                            customer.LastName = lastnameUpdate;

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Lastname updated");
                                            Console.ResetColor();
                                            Thread.Sleep(1000);
                                            db.SaveChanges();
                                            break;

                                        case 4:
                                            Console.Clear();
                                            Console.WriteLine($"Current Customer Age: {customer.Age}");
                                            Console.WriteLine("What do you want to update the Customer Age to?");
                                            string ageUpdate = Console.ReadLine()!;
                                            int age = 0;
                                            if (int.TryParse(ageUpdate, out age) && age > 0 && !string.IsNullOrWhiteSpace(ageUpdate))
                                            {
                                                customer.Age = age;
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("Age updated");
                                                Console.ResetColor();
                                                Thread.Sleep(1000);
                                                db.SaveChanges();
                                            }
                                            else
                                                Console.WriteLine("Invalid Input");
                                            break;

                                        case 5:
                                            Console.Clear();
                                            Console.WriteLine($"Current Customer Username: {customer.UserName}");
                                            Console.WriteLine("What do you want to update the Customer Lastname to?");
                                            string usernamUpdate = Console.ReadLine()!;
                                            customer.UserName = usernamUpdate;

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Username updated");
                                            Console.ResetColor();
                                            Thread.Sleep(1000);
                                            db.SaveChanges();
                                            break;

                                        case 6:
                                            Console.Clear();
                                            Console.WriteLine($"Current Customer Password: {customer.Password}");
                                            Console.WriteLine("What do you want to update the Customer Lastname to?");
                                            string passwordUpdate = Console.ReadLine()!;
                                            customer.Password = passwordUpdate;

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Password updated");
                                            Console.ResetColor();
                                            Thread.Sleep(1000);
                                            db.SaveChanges();
                                            break;

                                        case 7:
                                            Console.Clear();
                                            if (RunProgram.loggedinName == customer.UserName)
                                            {
                                                Console.WriteLine("You cant change Admin rights on your own account");
                                                Thread.Sleep(1500);
                                                break;
                                            }
                                            while (true)
                                            {
                                                Console.WriteLine($"Current Customer isAdmin: {customer.IsAdmin}");
                                                Console.WriteLine("What do you want to update the Customer isAdmin to? (true/false)");
                                                string isAdminUpdate = Console.ReadLine()!;
                                                if (isAdminUpdate.ToLower() == "true")
                                                {
                                                    customer.IsAdmin = true;
                                                    break;
                                                }
                                                else if (isAdminUpdate.ToLower() == "false")
                                                {
                                                    customer.IsAdmin = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid Input");
                                                    Thread.Sleep(1000);
                                                }
                                            }

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("isAdmin updated");
                                            Console.ResetColor();
                                            Thread.Sleep(1000);
                                            db.SaveChanges();
                                            break;
                                    }
                                }
                                else
                                    Console.WriteLine("Invalid Input");
                            }
                        }
                        else
                            Console.WriteLine("Invalid Input");

                    }
                    db.SaveChanges();
                    break;

                case 9:
                    Console.Clear();

                    var allOrders = db.Order.Include(a => a.Customer)
                        .Include(a => a.Products)
                        .GroupBy(x => x.Customer.UserName);

                    if (allOrders.Count() == 0)
                        Console.WriteLine("The orderlist is empty at the moment");
                    else
                    {
                        Console.Clear();
                        foreach (var orders in allOrders)
                        {
                            Console.WriteLine($"UserName: {orders.Key}");
                            Console.WriteLine("-------------------------------");
                            foreach (var item in orders)
                            {
                                Console.WriteLine($"OrderID: {item.Id}  Order created: {item.Date}  Payment: {item.PaymentChoice.PadRight(17)}\tCollected: {item.Shipping}");
                                Console.WriteLine("Products:");
                                foreach (var product in item.Products)
                                {
                                    Console.WriteLine($"Name: {product.Name.PadRight(48)} Amount: {product.Amount} - Price: {product.Price:C}");

                                }
                                Console.WriteLine($"------------------------------------------------------- Total: {item.TotalAmountPrice:C} ----------\n");
                            }
                        }
                    }

                    Console.WriteLine("");
                    Console.ReadKey();
                    break;

                case 10:
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("What do you want to show on top3?");
                        Console.WriteLine("1. Best selling products");
                        Console.WriteLine("2. Best Selling SubCategory/Maker");
                        Console.WriteLine("B to Back");
                        string stringTop = Console.ReadLine()!;
                        int intTop = 0;

                        if (stringTop.ToLower() == "b")
                            break;

                        var saveTop = db.Shop.SingleOrDefault();

                        if (int.TryParse(stringTop, out intTop) && intTop == 1)
                        {
                            var topSellers = db.Shop.OrderByDescending(x => x.Sold)
                                .Take(3).ToList();

                            foreach(var sell in topSellers)
                            {
                                saveTop.Top3.Add(new string($"Product Name: {sell.Name.PadRight(48)} Sold: {sell.Sold} Price: {sell.Price}"));
                            }

                            db.SaveChanges();
                        }
                        else if (int.TryParse(stringTop, out intTop) && intTop == 2)
                        {
                            var topSubcategory = db.Shop.OrderByDescending (x => x.SubCategory)
                                .Take(3).ToList();


                            db.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input");
                            Thread.Sleep(1500);
                        }
                    }

                    break;

                case 11:
                    Console.Clear();

                    while (true)
                    {
                        Console.Write("Type what you want to seach for: ");
                        string stringInput = Console.ReadLine()!;

                        if (stringInput.ToLower() == "b")
                            break;

                        if (!string.IsNullOrWhiteSpace(stringInput))
                        {
                            var search = db.Order.Include(a => a.Customer)
                                .Include(a => a.Products)
                                .Where(x =>
                                EF.Functions.Like(x.Name, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Date, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Id.ToString(), $"%{stringInput}%") ||
                                EF.Functions.Like(x.Zipcode.ToString(), $"%{stringInput}%") ||
                                EF.Functions.Like(x.ShippingFee.ToString(), $"%{stringInput}%") ||
                                EF.Functions.Like(x.City, $"%{stringInput}%") ||
                                EF.Functions.Like(x.PaymentChoice, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Adress, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Shipping, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.Name, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.LastName, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.UserName, $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.Id.ToString(), $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.LoggedIn.ToString(), $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.Age.ToString(), $"%{stringInput}%") ||
                                EF.Functions.Like(x.Customer.IsAdmin.ToString(), $"%{stringInput}%") ||
                                x.Products.Any(p => EF.Functions.Like(p.Id.ToString(), $"%{stringInput}%")) ||
                                x.Products.Any(p => EF.Functions.Like(p.Name, $"%{stringInput}%")) ||
                                x.Products.Any(p => EF.Functions.Like(p.Price.ToString(), $"%{stringInput}%")) ||
                                x.Products.Any(p => EF.Functions.Like(p.Amount.ToString(), $"%{stringInput}%")) ||
                                x.Products.Any(p => EF.Functions.Like(p.OrderId.ToString(), $"%{stringInput}%"))
                                )
                                .GroupBy(x => x.Customer.Id).ToList();

                            foreach (var key in search)
                            {
                                Console.WriteLine(key.Key);
                                foreach (var item in key)
                                {
                                    Console.WriteLine("Customer");
                                    Console.WriteLine($"CustomerId: {item.Customer.Id} Name: {item.Customer.Name} Lastname: {item.Customer.LastName} Username: {item.Customer.UserName} Age: {item.Customer.Age} LoggedIn: {item.Customer.LoggedIn} isAdmin: {item.Customer.IsAdmin}");
                                    Console.WriteLine("Order");
                                    Console.WriteLine($"OrderId: {item.Id} City: {item.City} Adress: {item.Adress} ZipCode: {item.Zipcode} Payments: {item.PaymentChoice} Shipping: {item.Shipping}");
                                    Console.WriteLine("Products");
                                    foreach (var product in item.Products)
                                    {
                                        Console.WriteLine($"Productid: {product.Id} Name: {product.Name} Price: {product.Price} OrderId: {product.OrderId}");
                                    }

                                    Console.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nothing found");
                            Thread.Sleep(1000);
                        }
                    }
                    break;
            }
        }
    }

    public static bool BackOption(string input)
    {
        if (input.ToLower() == "b") //  för att backa
            return true;

        return false;
    }
}
