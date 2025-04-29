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

            Console.Write("Please type the password: ");
            string passwordCheck = Console.ReadLine()!;

            if (passwordCheck == "apa")
            {
                bool adminRunning = true;
                while (adminRunning)
                {
                    int input = 0;
                    do
                    {
                        Console.Clear();

                        Console.WriteLine("What do you want to do?");
                        Console.WriteLine($"1. Add Item to shop");
                        Console.WriteLine($"2. Remove Item in shop");
                        Console.WriteLine($"3. Add or Remove stock for item");
                        Console.WriteLine($"4. Back");
                        input = int.Parse(Console.ReadLine()!);
                        if (input < 1 || input > 4)
                        {
                            Console.WriteLine("Must be numbers from 1-4 ");
                            Thread.Sleep(1000);
                        }

                    } while (input < 1 || input > 4);

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
                    switch (input)
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
                            adminRunning = false;
                            break;
                    }
                }
            }
        }
    }
}
