using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InlämningsUppgift_E_Tech_CO;
internal class Admin
{
    public static async Task AdminConsol(ICustomer customerInput)
    {
        // Här har jag skickat med ICustomer ifrån RunProgram så att jag inte skapar en ny för då blir inte värderna samma
        ICustomer isGuest = customerInput;

        using (var db = new MyDbContext())
        {
            // Testade göra en do while-loop här för att se vilken som blev bäst att använda
            while (true)
            {
                Console.Clear();

                bool validInput = false;
                int userInput = 0;
                Console.Clear();


                Console.WriteLine($"What do you want to do?");
                Console.WriteLine($"1.  Add Product to shop");
                Console.WriteLine($"2.  Remove Product in shop");
                Console.WriteLine($"3.  Increase/Decrease stock for items");
                Console.WriteLine($"4.  Change price for Product");
                Console.WriteLine($"5.  Change Category/subcategory");
                Console.WriteLine($"6.  Delete Category/subcategory");
                Console.WriteLine($"7.  Change Name on Product");
                Console.WriteLine($"8.  Product info");
                Console.WriteLine($"9.  Change Shipping fee");
                Console.WriteLine($"10. All customers & Change Customer");
                Console.WriteLine($"11. Look Orderhistories");                    
                Console.WriteLine($"12. Change top3 in menu");
                Console.WriteLine($"B to Back");
                string input = Console.ReadLine()!;

                if (BackOption(input))
                    break;

                var categorySearch = await db.Shop.OrderBy(i => i.Id)
                                            .GroupBy(c => new { c.Category, c.SubCategory })
                                            .ToListAsync();
                if (int.TryParse(input, out userInput) && userInput > 0 && userInput < 13)
                {

                    if (userInput > 0 && userInput < 9)
                    {
                        Console.Clear();


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
                                Console.Write("Which Category do you want to add this item to?: ");
                                string category = Console.ReadLine()!;
                                if (category.ToLower() == "b")
                                    break;

                                Console.Write("Which Subcategory/product maker do you want to add this item to?: ");
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

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Product added to Store");
                                Console.ResetColor();
                                Thread.Sleep(1500);
                                break;
                            }
                            db.SaveChanges();
                            break;

                        case 2:
                            while (true)
                            {
                                int deleteId = 0;
                                Console.Write("Which product do you want to delete? or [B]ack: ");
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
                            while (true)
                            {
                                int updateStock = 0;
                                Console.Write("Which product do u want to alter the stock? or [B]ack: ");
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
                            while (true)
                            {
                                int updatePrice = 0;
                                Console.Write("Which product do u want to change price on? or [B]ack: ");
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
                            while (true)
                            {
                                int updateCategory = 0;
                                Console.Write($"Which Product do you want to change category/subcategory on? or [B]ack: ");
                                string catSubCheck = Console.ReadLine()!;

                                if (BackOption(catSubCheck))
                                    break;

                                if (int.TryParse(catSubCheck, out updateCategory) && updateCategory > 0 && !string.IsNullOrWhiteSpace(catSubCheck))
                                {
                                    var categoryAndSub = db.Shop.Where(x => x.Id == updateCategory).SingleOrDefault();

                                    Console.Write($"Which Category do u want to change to?: ");
                                    string categoryChange = Console.ReadLine()!;
                                    Console.Write($"Which Subcategory do u want to change to?: ");
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
                            while (true)
                            {
                                Console.Clear();

                                Console.WriteLine("What do you want to Delete?");
                                Console.WriteLine("1. To Delete a Category");
                                Console.WriteLine("2. To Delete a Subategory");
                                Console.WriteLine("B to Back");
                                string stringDeleteCategory = Console.ReadLine()!;
                                int intDeleteCategory = 0;

                                if (stringDeleteCategory.ToLower() == "b")
                                    break;

                                if (int.TryParse(stringDeleteCategory, out intDeleteCategory))
                                {
                                    if (intDeleteCategory == 1)
                                    {
                                        while (true)
                                        {
                                            int counter = 1;
                                            Console.Clear();
                                            var allCategory = db.Shop.GroupBy(x => x.Category).ToList();

                                            Console.WriteLine("B to back");
                                            Console.WriteLine("Which do you want to Delete?");
                                            foreach (var cate in allCategory)
                                            {
                                                Console.WriteLine($"{counter}.{cate.Key}");
                                                counter++;
                                            }
                                            string inputDelete = Console.ReadLine()!;
                                            int intInputDelete = 0;
                                            if (inputDelete.ToLower() == "b")
                                                break;

                                            if (int.TryParse(inputDelete, out intInputDelete) && intInputDelete > 0 && intInputDelete <= allCategory.Count)
                                            {
                                                var selectedCategory = allCategory[intInputDelete - 1].Key;

                                                var cateToDelete = db.Shop.Where(x => x.Category == selectedCategory).SingleOrDefault();
                                                db.Shop.Remove(cateToDelete);
                                                db.SaveChanges();

                                                Console.WriteLine("Category Deleted");
                                                Thread.Sleep(1500);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Input");
                                                Thread.Sleep(1560);
                                            }
                                        }
                                    }
                                    else if (intDeleteCategory == 2)
                                    {
                                        while (true)
                                        {
                                            int counter = 1;
                                            Console.Clear();
                                            var allSubcategory = db.Shop.GroupBy(x => x.SubCategory).ToList();

                                            Console.WriteLine("B to back");
                                            Console.WriteLine("Which do you want to Delete?");
                                            foreach (var cate in allSubcategory)
                                            {
                                                Console.WriteLine($"{counter}.{cate.Key}");
                                                counter++;
                                            }
                                            string inputDelete = Console.ReadLine()!;
                                            int intInputDelete = 0;

                                            if (inputDelete.ToLower() == "b")
                                                break;

                                            if (int.TryParse(inputDelete, out intInputDelete) && intInputDelete > 0 && intInputDelete <= allSubcategory.Count)
                                            {
                                                var selectedCategory = allSubcategory[intInputDelete - 1].Key;

                                                var cateToDelete = db.Shop.Where(x => x.SubCategory == selectedCategory).SingleOrDefault();
                                                db.Shop.Remove(cateToDelete);
                                                db.SaveChanges();

                                                Console.WriteLine("Subcategory Deleted");
                                                Thread.Sleep(1500);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Input");
                                                Thread.Sleep(1560);
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
                            //db.SaveChanges();
                            break;

                        case 7:
                            while (true)
                            {
                                int updateProductName = 0;
                                Console.Write($"Which Product do you want to change Name on? or [B]ack: ");
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

                        case 8:
                            while (true)
                            {
                                int updateProductInformation = 0;

                                Console.Write($"Which product do you want to alter the information about? or [B]ack: ");
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

                        case 9:
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("1. to change Regular shipping fee");
                                Console.WriteLine("2. to change Express shipping fee");
                                Console.WriteLine("B to back");
                                string shippingCheck = Console.ReadLine()!;
                                int intShippingCheck = 0;

                                if (shippingCheck.ToLower() == "b")
                                    break;

                                if (int.TryParse(shippingCheck, out intShippingCheck))
                                {
                                    while (true)
                                    {
                                        var shippingFee = db.Shop.ToList();

                                        Console.Clear();
                                        if (intShippingCheck == 1)
                                        {
                                            int? current = 0;
                                            foreach (var fee in db.Shop)
                                            {
                                                current = fee.RegularShipping;
                                                break;
                                            }
                                            Console.WriteLine("B to back");
                                            Console.WriteLine($"Current Regular shipping fee: {current}");
                                            Console.Write($"Switch to?: ");
                                            string feeChange = Console.ReadLine()!;
                                            int intFeeChange = 0;

                                            if (feeChange.ToLower() == "b")
                                                break;

                                            if (int.TryParse(feeChange, out intFeeChange) && intFeeChange >= 0)
                                            {

                                                Console.WriteLine("Regulare shipping fee changed");
                                                foreach (var fee in db.Shop)
                                                {
                                                    fee.RegularShipping = intFeeChange;
                                                }
                                                db.SaveChanges();
                                                Thread.Sleep(1500);
                                                break;
                                            }
                                        }
                                        else if (intShippingCheck == 2)
                                        {
                                            int? current = 0;
                                            foreach (var fee in db.Shop)
                                            {
                                                current = fee.ExpressShipping;
                                                break;
                                            }
                                            Console.WriteLine("B to back");
                                            Console.WriteLine($"Current Express shipping fee: {current}");
                                            Console.Write($"Switch to?: ");
                                            string feeChange = Console.ReadLine()!;
                                            int intFeeChange = 0;

                                            if (feeChange.ToLower() == "b")
                                                break;

                                            if (int.TryParse(feeChange, out intFeeChange) && intFeeChange >= 0)
                                            {
                                                Console.WriteLine("Express shipping fee changed");
                                                foreach (var fee in db.Shop)
                                                {
                                                    fee.ExpressShipping = intFeeChange;
                                                }
                                                db.SaveChanges();
                                                Thread.Sleep(1500);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Inavlid Input");
                                            Thread.Sleep(1500);
                                        }
                                    }
                                }
                            }

                            break;

                        case 10:
                            while (true)
                            {
                                Console.Clear();
                                int updateCustomerInformation = 0;
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
                                Console.WriteLine("Which Person do you want to update/delete");
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
                                    }
                                    else
                                    {

                                        while (true)
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
                                            string updateCustomerString = Console.ReadLine()!;
                                            if (BackOption(updateCustomerString))
                                                break;


                                            int number = 0;
                                            if (int.TryParse(updateCustomerString, out number) && !string.IsNullOrWhiteSpace(updateCustomerString) && number > 0)
                                            {
                                                switch (number)
                                                {
                                                    case 1:
                                                        Console.Clear();
                                                        if (isGuest.UserName == customer.UserName)
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
                                                        while (true)
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine($"Current Customer Name: {customer.Name}");
                                                            Console.WriteLine("What do you want to update the Customer Name to?");
                                                            Console.WriteLine("B to back");
                                                            string nameUpdate = Console.ReadLine()!;

                                                            if (nameUpdate.ToLower() == "b")
                                                                break;

                                                            customer.Name = nameUpdate;

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("Name updated");
                                                            Console.ResetColor();
                                                            Thread.Sleep(1000);
                                                            db.SaveChanges();
                                                        }
                                                        break;

                                                    case 3:
                                                        while (true)
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine($"Current Customer lastname: {customer.LastName}");
                                                            Console.WriteLine("What do you want to update the Customer Lastname to?");
                                                            Console.WriteLine("B to back");
                                                            string lastnameUpdate = Console.ReadLine()!;

                                                            if (lastnameUpdate.ToLower() == "b")
                                                                break;

                                                            customer.LastName = lastnameUpdate;

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("Lastname updated");
                                                            Console.ResetColor();
                                                            Thread.Sleep(1000);
                                                            db.SaveChanges();
                                                        }
                                                        break;

                                                    case 4:
                                                        while (true)
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine($"Current Customer Age: {customer.Age}");
                                                            Console.WriteLine("What do you want to update the Customer Age to?");
                                                            Console.WriteLine("B to back");
                                                            string ageUpdate = Console.ReadLine()!;

                                                            if (ageUpdate.ToLower() == "b")
                                                                break;

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
                                                        }

                                                        break;

                                                    case 5:
                                                        while (true)
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine($"Current Customer Username: {customer.UserName}");
                                                            Console.WriteLine("What do you want to update the Customer Username to?");
                                                            Console.WriteLine("B to back");
                                                            string usernamUpdate = Console.ReadLine()!;

                                                            if (usernamUpdate.ToLower() == "b")
                                                                break;

                                                            customer.UserName = usernamUpdate;

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("Username updated");
                                                            Console.ResetColor();
                                                            Thread.Sleep(1000);
                                                            db.SaveChanges();
                                                        }
                                                        break;

                                                    case 6:
                                                        while (true)
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine($"Current Customer Password: {customer.Password}");
                                                            Console.WriteLine("What do you want to update the Customer Lastname to?");
                                                            Console.WriteLine("B to back");
                                                            string passwordUpdate = Console.ReadLine()!;

                                                            if (passwordUpdate.ToLower() == "b")
                                                                break;

                                                            customer.Password = passwordUpdate;

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("Password updated");
                                                            Console.ResetColor();
                                                            Thread.Sleep(1000);
                                                            db.SaveChanges();
                                                        }
                                                        break;

                                                    case 7:

                                                        Console.Clear();
                                                        if (isGuest.UserName == customer.UserName)
                                                        {
                                                            Console.WriteLine("You cant change Admin rights on your own account");
                                                            Thread.Sleep(1500);
                                                            break;
                                                        }
                                                        while (true)
                                                        {
                                                            Console.WriteLine($"Current Customer isAdmin: {customer.IsAdmin}");
                                                            Console.WriteLine("What do you want to update the Customer isAdmin to? (true/false)");
                                                            Console.WriteLine("B to back");
                                                            string isAdminUpdate = Console.ReadLine()!;

                                                            if (isAdminUpdate.ToLower() == "b")
                                                                break;

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

                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("isAdmin updated");
                                                            Console.ResetColor();
                                                            Thread.Sleep(1000);
                                                            db.SaveChanges();

                                                        }
                                                        break;
                                                }

                                            }
                                            else
                                                Console.WriteLine("Invalid Input");
                                        }
                                    }
                                }
                                else
                                    Console.WriteLine("Invalid Input");
                            }
                            db.SaveChanges();
                            break;

                        case 11:
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
                                        Console.WriteLine($"------ Shipping: {item.ShippingFee:C} ------ Taxes: {Convert.ToInt32((item.TotalAmountPrice - item.ShippingFee) * 0.25):C} ------ Total Price: {item.TotalAmountPrice:C} ------");
                                    }
                                }
                            }

                            Console.WriteLine("");
                            Console.ReadKey();
                            break;

                        case 12:
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("What do you want to show on top3?");
                                Console.WriteLine("1. Best selling products");
                                Console.WriteLine("2. Best Selling SubCategory/Maker");
                                Console.WriteLine("B to Back");
                                string stringTop = Console.ReadLine()!;
                                int intTop = 0;

                                if (stringTop.ToLower() == "b")
                                    break;



                                var resetTop = db.Shop.OrderByDescending(x => x.IsActiveCategory)
                                    .ToList();

                                if (int.TryParse(stringTop, out intTop) && intTop == 1)
                                {
                                    var topSellers = db.Shop
                                        .OrderByDescending(x => x.Sold)
                                        .Take(3).ToList();

                                    foreach (var reset in resetTop)
                                    {
                                        reset.IsActive = false;
                                        reset.IsActiveCategory = null;
                                    }
                                    db.SaveChanges();

                                    foreach (var sell in topSellers)
                                    {
                                        sell.IsActive = true;
                                        sell.IsActiveCategory = 1;
                                    }
                                    db.SaveChanges();

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Top3 Succefully updated");
                                    Console.ResetColor();
                                    Thread.Sleep(1500);
                                }
                                else if (int.TryParse(stringTop, out intTop) && intTop == 2)
                                {
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

                                    var orderedList = topCategories.OrderByDescending(x => x.totalSold);

                                    foreach (var reset in resetTop)
                                    {
                                        reset.IsActive = false;
                                        reset.IsActiveCategory = null;
                                    }
                                    db.SaveChanges();

                                    foreach (var top in orderedList)
                                    {
                                        var toUpdate = db.Shop.Where(x => x.SubCategory == top.Category).ToList();

                                        foreach (var item in toUpdate)
                                        {
                                            item.IsActive = true;
                                            item.IsActiveCategory = 2;
                                        }
                                    }
                                    db.SaveChanges();

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Top3 Succefully updated");
                                    Console.ResetColor();
                                    Thread.Sleep(1500);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input");
                                    Thread.Sleep(1500);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                    Thread.Sleep(1000);
                }
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
