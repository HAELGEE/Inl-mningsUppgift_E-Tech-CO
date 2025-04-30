using InlämningsUppgift_E_Tech_CO.Models;
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

                        Console.WriteLine("What do you want to do?");
                        Console.WriteLine($"1.  Add Item to shop");
                        Console.WriteLine($"2.  Remove Item in shop");
                        Console.WriteLine($"3.  Increase stock for items");
                        Console.WriteLine($"4.  Decrease stock for items");
                        Console.WriteLine($"5.  Change price for item");
                        Console.WriteLine($"6.  Change Category/subcategory");
                        Console.WriteLine($"7.  Product info");
                        Console.WriteLine($"8.  Provider");
                        Console.WriteLine($"9.  Change customer information");
                        Console.WriteLine($"10. Look Orderhistory");
                        Console.WriteLine($"11. Back");
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
                    // lägga ut en foreach-loop där man skriver ut alla items så man ser Vad man har så man vet Vad man kan uppdatera eller lägga till

                    var categorySearch = db.Shop.GroupBy(c => new { c.Category, c.SubCategory });
                        
                    foreach (var cat in categorySearch)
                    {
                        Console.WriteLine($"Category: {cat.Key.Category}");
                        Console.WriteLine($"  SubCategory: {cat.Key.SubCategory}");
                        Console.WriteLine("-----------------------");
                        foreach (var item in cat)
                        {
                            Console.WriteLine($"ID:{item.Id} \t Name: {item.Name}\t in Stock: {item.Stock}");
                        }
                    }
                    Console.WriteLine();
                    switch (userInput)
                    {
                        case 1:                            

                            Console.Write("Wich Category do you want to add this item to?: ");
                            string category = Console.ReadLine()!;
                            Console.Write("Wich Subcategory do you want to add this item to?: ");
                            string subCategory = Console.ReadLine()!;
                            Console.Write("What is the name of the product?: ");
                            string productName = Console.ReadLine()!;
                            Console.Write("How many in stock?: ");
                            int stock = int.Parse(Console.ReadLine()!);
                            Console.Write("Enter information about the product: ");
                            string information = Console.ReadLine()!;

                            db.Shop.Add(new Shop
                            {
                                Category = category,
                                SubCategory = subCategory,
                                Name = productName,
                                Stock = stock,
                                ProductInformation = information
                            });

                            db.SaveChanges();
                            break;

                        case 2:
                            Console.WriteLine();

                            db.SaveChanges();
                            break;

                        case 3:

                            db.SaveChanges();
                            break;
                        case 4:

                            db.SaveChanges();
                            break;
                        case 5:

                            db.SaveChanges();
                            break;
                        case 6:

                            db.SaveChanges();
                            break;
                        case 7:

                            db.SaveChanges();
                            break;
                        case 8:

                            db.SaveChanges();
                            break;
                        case 9:

                            db.SaveChanges();
                            break;
                        case 10:

                            db.SaveChanges();
                            break;

                        case 11:
                            adminRunning = false;
                            break;
                    }
                }
            }
        }
    }
}
