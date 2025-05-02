using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class Admin
{
    public static void AdminConsol()
    {
        using (var db = new MyDbContext())
        {
            Console.Clear();

            Console.Write("Please type the correct password: ");
            string passwordCheck = Console.ReadLine()!;

            if (passwordCheck == "apa")
            {
                bool adminRunning = true;
                while (adminRunning)
                {
                    bool validInput = false;
                    int userInput = 0;
                    do
                    {
                        Console.Clear();

                        Console.WriteLine($"What do you want to do?");                  // klar
                        Console.WriteLine($"1.  Add Item to shop");                     // klar (nästan)
                        Console.WriteLine($"2.  Remove Item in shop");                  // klar
                        Console.WriteLine($"3.  Increase/Decrease stock for items");    // klar
                        Console.WriteLine($"4.  Change price for item");                // klar
                        Console.WriteLine($"5.  Change Category/subcategory");          // klar
                        Console.WriteLine($"6.  Product info");                         // klar
                        Console.WriteLine($"7.  Provider");
                        Console.WriteLine($"8.  Change customer information");
                        Console.WriteLine($"9.  Look Orderhistory");                    // Inte riktigt klar, får det inte utskrivet
                        Console.WriteLine($"10. Back");                                 // klar
                        string input = Console.ReadLine()!;

                        if (int.TryParse(input, out userInput) && userInput >= 1 && userInput <= 11)
                            validInput = true;
                        else
                        {
                            Console.WriteLine("Must be numbers from 1-11 ");
                            Thread.Sleep(1000);
                        }

                    } while (!validInput);

                    Console.Clear();

                    var categorySearch = db.Shop.GroupBy(c => new { c.Category, c.SubCategory });

                    foreach (var cat in categorySearch)
                    {
                        Console.WriteLine($"Category: {cat.Key.Category}");
                        Console.WriteLine($"  SubCategory: {cat.Key.SubCategory}");
                        Console.WriteLine("-----------------------");
                        foreach (var item in cat)
                        {
                            Console.WriteLine($"ID:{item.Id} \t Name: {item.Name}\t in Stock: {item.Stock}, Price: {item.Price}");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    switch (userInput)
                    {
                        case 1:
                            Console.WriteLine("Press [q] to Quit");
                            Console.Write("Wich Category do you want to add this item to?: ");
                            string category = Console.ReadLine()!;
                            Console.Write("Wich Subcategory do you want to add this item to?: ");
                            string subCategory = Console.ReadLine()!;
                            Console.Write("What is the name of the product?: ");
                            string productName = Console.ReadLine()!;
                            Console.Write("How much is the price?: ");

                            double productPrice = double.Parse(Console.ReadLine()!);
                            Console.Write("How many in stock?: ");
                            int stock = int.Parse(Console.ReadLine()!);

                            Console.Write("Enter information about the product: ");
                            string information = Console.ReadLine()!;

                            if (category == "q" || subCategory == "q" || productName == "q" || productName == "q" || information == "q")
                                break;

                            db.Shop.Add(new Shop
                            {
                                Category = category,
                                SubCategory = subCategory,
                                Name = productName,
                                Price = productPrice,
                                Stock = stock,
                                ProductInformation = information
                            });

                            db.SaveChanges();
                            break;

                        case 2:
                            int deleteId = 0;
                            while (deleteId <= 0)
                            {
                                Console.Write("Wich product do you want to delete? or [Q]uit: ");
                                string deleteCheck = Console.ReadLine()!;

                                if (deleteCheck == "q")
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
                            while (updateStock <= 0)
                            {
                                Console.Write("Wich product do u want to alter the stock? or [Q]uit: ");
                                string updateCheck = Console.ReadLine()!;

                                if (updateCheck == "q")
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
                                            if (updateItem.Stock > 0)
                                                updateItem.Stock = updateItem.Stock + updateStock;
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
                                Console.Write("Wich product do u want to change price on? or [Q]uit: ");
                                string priceCheck = Console.ReadLine()!;

                                if (priceCheck == "q")
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
                                Console.Write($"Wich Product do you want to change category/subcategory on? or [Q]uit: ");
                                string catSubCheck = Console.ReadLine()!.ToLower();

                                if (catSubCheck == "q")
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
                            int updateProductInformation = 0;
                            while (updateProductInformation <= 0)
                            {
                                Console.Write($"Wich product do you want to alter the information about? or [Q]uit: ");
                                string productAlter = Console.ReadLine()!.ToLower();

                                if (productAlter == "q")
                                    break;

                                if(int.TryParse(productAlter, out updateProductInformation) && updateProductInformation > 0 && !string.IsNullOrWhiteSpace(productAlter))
                                {
                                    Console.Clear();

                                    var productInfo = db.Shop.Where(x => x.Id == updateProductInformation).SingleOrDefault();
                                    Console.WriteLine($"Product: {productInfo.Name}");
                                    Console.Write($"Information about the product: ");
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine(productInfo.ProductInformation + "\n");
                                    Console.ResetColor();
                                    Console.WriteLine("What do tou want to update the information to? or [Q]uit: ");
                                    string checkProductInfo = Console.ReadLine()!.ToLower();

                                    if (checkProductInfo == "q")
                                        break;

                                    productInfo.ProductInformation = checkProductInfo;
                                }
                            }
                            db.SaveChanges();
                            break;
                        case 7:
                            int updateProvider = 0;
                            while (updateProvider <= 0)
                            {

                            }
                            db.SaveChanges();
                            break;
                        case 8:
                            int updateCustomerInformation = 0;
                            while (updateCustomerInformation <= 0)
                            {

                            }
                            db.SaveChanges();
                            break;
                        case 9:
                            Console.Clear();

                            var allOrders = db.OrderHistories   .Join(db.Shop, order => order.Id, shop => shop.Id, (order, shop) => new
                                                                {
                                                                   OrderHistory = order,
                                                                   Shop = shop,
                                                                }).ToList()
                                                                .OrderBy(x => x.OrderHistory.Id);
                            
                            foreach (var orders in allOrders)
                            {
                                Console.WriteLine($"ID: {orders.OrderHistory.Id}\t {orders.Shop.Name} \t {orders.Shop.Sold}");
                            }

                            Console.ReadKey();
                            break;

                        case 10:
                            adminRunning = false;
                            break;
                    }
                }
            }
        }
    }
}
