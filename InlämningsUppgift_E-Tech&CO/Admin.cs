using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
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

            Console.Write("Please type the correct password: ");
            string passwordCheck = Console.ReadLine()!;

            if (passwordCheck == "apa")
            {
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
                    Console.WriteLine($"B to Back");
                    string input = Console.ReadLine()!;

                    if (BackOption(input))
                        break;

                    if (int.TryParse(input, out userInput) && userInput >= 1 && userInput <= 9)
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
                        Console.WriteLine("Press [B] to back");
                        Console.Write("Wich Category do you want to add this item to?: ");
                        string category = Console.ReadLine()!;
                        Console.Write("Wich Subcategory/product maker do you want to add this item to?: ");
                        string subCategory = Console.ReadLine()!;
                        Console.Write("What is the name of the product?: ");
                        string productName = Console.ReadLine()!;
                        Console.Write("Price on Product?: ");

                        double productPrice = double.Parse(Console.ReadLine()!);
                        Console.Write("How many in stock?: ");
                        int stock = int.Parse(Console.ReadLine()!);

                        Console.Write("Enter information about the product: ");
                        string information = Console.ReadLine()!;

                        if (category == "b" || subCategory == "b" || productName == "b" || productName == "b" || information == "b")
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
                                    Console.WriteLine($"Name: {customer.Name}   Lastname: {customer.LastName}   Age: {customer.Age}   Username: {customer.UserName}   Password: {customer.Password}\n");
                                    Console.ResetColor();
                                    Console.WriteLine("1. Delete Customer");
                                    Console.WriteLine("2. Update Customer Name");
                                    Console.WriteLine("3. Update Customer Lastname");
                                    Console.WriteLine("4. Update Customer Age");
                                    Console.WriteLine("5. Update Customer Username");
                                    Console.WriteLine("6. Update Customer Password");
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

                        var allOrders = await db.OrderHistories.Join(db.Shop, order => order.Id, shop => shop.Id, (order, shop) => new
                        {
                            OrderHistory = order,
                            Shop = shop,
                        })
                          .OrderBy(x => x.OrderHistory.Id)
                          .ToListAsync();

                        if (allOrders.Count() == 0)
                            Console.WriteLine("The orderlist is empty at the moment");
                        else
                            foreach (var orders in allOrders)
                            {
                                Console.WriteLine($"ID: {orders.OrderHistory.Id}\t {orders.Shop.Name} \t {orders.Shop.Sold}");
                            }

                        Console.ReadKey();
                        break;
                }

            }
        }
    }
    static public bool BackOption(string input)
    {
        if (input.ToLower() == "b") //  för att backa
            return true;

        return false;
    }
}
