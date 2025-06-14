﻿using BCrypt.Net;
using InlämningsUppgift_E_Tech_CO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.VisualBasic;
using System;
using System.Collections;
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
            while (true)
            {
                int userInput = 0;
                Console.Clear();

                Console.WriteLine($"What do you want to do?");
                Console.WriteLine($"1.  Add Product to shop");
                Console.WriteLine($"2.  Remove Product in shop");
                Console.WriteLine($"3.  Increase/Decrease stock for items");
                Console.WriteLine($"4.  Change price for Product");
                Console.WriteLine($"5.  Change Name on Product");
                Console.WriteLine($"6.  Product info");
                Console.WriteLine($"7.  Add Category/subcategory");
                Console.WriteLine($"8.  Change Category/subcategory");
                Console.WriteLine($"9.  Delete Category/subcategory");
                Console.WriteLine($"10. Change Shipping fee");
                Console.WriteLine($"11. All customers & Change Customer");
                Console.WriteLine($"12. Look Orderhistories");
                Console.WriteLine($"13. Change top3 in menu");
                Console.WriteLine($"B to Back");
                string input = Console.ReadLine()!;

                if (BackOption(input))
                    break;

                //var gettingProducts = db.Shop.ToList().GroupBy(x => new { x.ProductCategoryId, x.ProductSubcategoryId });

                if (int.TryParse(input, out userInput) && userInput > 0)
                {
                    switch (userInput)
                    {
                        case 1:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

                                string category = "";
                                int intcategory = 0;
                                while (true)
                                {
                                    Console.WriteLine("Press [B] to back");
                                    Console.Write("Which Category do you want to add this item to? Chose ID: ");
                                    category = Console.ReadLine()!;

                                    if (category.ToLower() == "b")
                                        break;

                                    if (int.TryParse(category, out intcategory))
                                        break;
                                    else
                                        RunProgram.ErrorMessage();

                                }
                                if (category.ToLower() == "b")
                                    break;

                                string subCategory = "";
                                int intsubCategory = 0;
                                while (true)
                                {
                                    Console.Write("Which Subcategory/product maker do you want to add this item to? Chose ID: ");
                                    subCategory = Console.ReadLine()!;

                                    if (subCategory.ToLower() == "b")
                                        break;


                                    if (int.TryParse(subCategory, out intsubCategory))
                                        break;
                                    else
                                        RunProgram.ErrorMessage();
                                }
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
                                        RunProgram.ErrorMessage();

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
                                        RunProgram.ErrorMessage();
                                }
                                if (stockString.ToLower() == "b")
                                    break;

                                Console.Write("Enter information about the product: ");
                                string information = Console.ReadLine()!;
                                if (information.ToLower() == "b")
                                    break;


                                var categoryID = db.ProductCategory.Where(x => x.Id == intcategory)
                                    .SingleOrDefault();

                                var subcategoryID = db.ProductSubcategory.Where(x => x.Id == intsubCategory)
                                    .SingleOrDefault();


                                var categoryUpdate = db.ProductCategory.Where(x => x.Id == intcategory)
                                                .Select(x => new { x.Id }).SingleOrDefault();

                                var categoryCheck = db.ProductSubcategory.Where(x => x.Id == intsubCategory)
                                                      .Select(x => new { x.Id, x.ProductCategoryId }).SingleOrDefault();


                                if (categoryID != null && subcategoryID != null && categoryUpdate!.Id == categoryCheck!.ProductCategoryId)
                                {
                                    db.Shop.Add(new Shop
                                    {
                                        ProductCategoryId = categoryID.Id,
                                        ProductSubcategoryId = subcategoryID.Id,
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
                                else
                                {
                                    RunProgram.ChangeColor($"\nSomething went wrong. Check your spelling and if there is a Category with that Spelling", "Red");
                                    Thread.Sleep(1500);
                                }
                            }
                            db.SaveChanges();
                            break;

                        case 2:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

                                int deleteId = 0;
                                Console.Write("Which product do you want to delete? or [B]ack: ");
                                string deleteCheck = Console.ReadLine()!;

                                if (BackOption(deleteCheck))
                                    break;

                                if (int.TryParse(deleteCheck, out deleteId) && deleteId > 0 && !string.IsNullOrWhiteSpace(deleteCheck))
                                {
                                    var deleteItem = db.Shop.Where(x => x.Id == deleteId).SingleOrDefault();
                                    db.Shop.Remove(deleteItem!);
                                    db.SaveChanges();

                                    RunProgram.ChangeColor($"Product succefully deleted", "Green");
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }

                            break;

                        case 3:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

                                int updateStock = 0;
                                Console.Write("Which product do u want to alter the stock? or [B]ack: ");
                                string updateCheck = Console.ReadLine()!;

                                if (BackOption(updateCheck))
                                    break;

                                if (int.TryParse(updateCheck, out updateStock) && updateStock > 0 && !string.IsNullOrWhiteSpace(updateCheck))
                                {
                                    var updateItem = db.Shop.Where(x => x.Id == updateStock).SingleOrDefault();
                                    updateStock = 0;
                                    while (true)
                                    {
                                        Console.Write($"How much do you want to alter?: ");
                                        string alterCheck = Console.ReadLine()!;
                                        if (int.TryParse(alterCheck, out updateStock) && updateStock != 0)
                                        {
                                            if (updateItem!.Quantity >= 0)
                                            {
                                                updateItem.Quantity = updateItem.Quantity + updateStock;
                                                RunProgram.ChangeColor("Product Stock have been succefully changed", "Green");
                                                Thread.Sleep(1000);
                                                break;
                                            }
                                            else
                                                Console.WriteLine("You cant have negative in your balance");
                                        }
                                    }
                                    break;
                                }
                            }

                            db.SaveChanges();
                            break;
                        case 4:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

                                Console.Write("Which product do u want to change price on? or [B]ack: ");
                                string priceCheck = Console.ReadLine()!;
                                int intPriceCheck = 0;

                                if (BackOption(priceCheck))
                                    break;


                                if (int.TryParse(priceCheck, out intPriceCheck) && intPriceCheck > 0)
                                {
                                    var updateItem = db.Shop.Where(x => x.Id == intPriceCheck).SingleOrDefault();
                                    Console.WriteLine("What do you want to change the price to?");
                                    string updatePrice = Console.ReadLine()!;
                                    int intUpdatePrice = 0;

                                    if (int.TryParse(updatePrice, out intUpdatePrice) && intUpdatePrice > 0)
                                    {
                                        updateItem!.Price = intUpdatePrice;

                                        RunProgram.ChangeColor("Product Price have been succefully changed", "Green");
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                    else
                                        Console.WriteLine("Cant be negative in price");
                                }
                            }
                            db.SaveChanges();
                            break;

                        case 5:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

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
                                    productNameUpdate!.Name = productNameInfo;
                                    RunProgram.ChangeColor("Product Name have been succefully changed", "Green");
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }

                            db.SaveChanges();
                            break;

                        case 6:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

                                int updateProductInformation = 0;

                                Console.Write($"Which product do you want to alter the information about? or [B]ack: ");
                                string productAlter = Console.ReadLine()!;

                                if (BackOption(productAlter))
                                    break;

                                if (int.TryParse(productAlter, out updateProductInformation) && updateProductInformation > 0 && !string.IsNullOrWhiteSpace(productAlter))
                                {
                                    Console.Clear();

                                    var productInfo = db.Shop.Where(x => x.Id == updateProductInformation).SingleOrDefault();
                                    Console.WriteLine($"Product: {productInfo!.Name}");
                                    Console.Write($"Information about the product: ");
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine(productInfo.ProductInformation + "\n");
                                    Console.ResetColor();
                                    Console.WriteLine("What do tou want to update the information to? or [B]ack: ");
                                    string checkProductInfo = Console.ReadLine()!;

                                    if (BackOption(checkProductInfo))
                                        break;

                                    if (!string.IsNullOrWhiteSpace(checkProductInfo))
                                    {
                                        productInfo.ProductInformation = checkProductInfo;
                                        RunProgram.ChangeColor("Product Info have been succefully changed", "Green");
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }
                            db.SaveChanges();
                            break;

                        case 7:
                            while (true)
                            {
                                Console.Clear();

                                Console.WriteLine("1. To add new Category");
                                Console.WriteLine("2. To add new Subcategory");
                                Console.WriteLine("3. B to Back");
                                string addCategory = Console.ReadLine()!;
                                int intaddCategory = 0;

                                if (BackOption(addCategory))
                                    break;

                                if (int.TryParse(addCategory, out intaddCategory) && intaddCategory > 0)
                                {
                                    Console.Clear();

                                    if (intaddCategory == 1)
                                    {
                                        Console.WriteLine("What do you want to name the Category?");
                                        string? newCategory = Console.ReadLine()!;

                                        db.ProductCategory.Add(new ProductCategory
                                        {
                                            ProductCategoryName = newCategory
                                        });

                                        db.SaveChanges();
                                        break;
                                    }
                                    else if (intaddCategory == 2)
                                    {
                                        var printCategories = db.ProductCategory.ToList();
                                        foreach (var category in printCategories)
                                        {
                                            Console.WriteLine($"Id. {category.Id} - {category.ProductCategoryName}");
                                        }
                                        Console.WriteLine("Which Category do you want to add this Subcategory?");
                                        string? categoryCheck = Console.ReadLine()!;
                                        int intcategoryCheck = 0;

                                        if (int.TryParse(categoryCheck, out intcategoryCheck))
                                        {
                                            var allCategories = db.ProductCategory.Where(x => x.Id == intcategoryCheck).SingleOrDefault();

                                            Console.Clear();

                                            Console.WriteLine($"{allCategories!.ProductCategoryName} is Selected");
                                            Console.WriteLine("What do you want to name the Subcategory?");
                                            string newSubcategory = Console.ReadLine()!;


                                            db.ProductSubcategory.Add(new ProductSubcategory
                                            {
                                                ProductCategoryId = intcategoryCheck,
                                                ProductSubcategoryName = newSubcategory
                                            });
                                            db.SaveChanges();
                                            break;

                                        }
                                        else
                                            RunProgram.ErrorMessage();
                                    }
                                    else
                                        RunProgram.ErrorMessage();
                                }
                                else
                                    RunProgram.ErrorMessage();
                            }
                            break;

                        case 8:
                            while (true)
                            {
                                Console.Clear();
                                Categorylist();

                                int updateCategory = 0;
                                Console.Write($"Which Product do you want to change category/subcategory on? or [B]ack: ");
                                string catSubCheck = Console.ReadLine()!;

                                if (BackOption(catSubCheck))
                                    break;

                                if (int.TryParse(catSubCheck, out updateCategory) && updateCategory > 0 && !string.IsNullOrWhiteSpace(catSubCheck))
                                {
                                    var productToAlter = db.Shop.Where(x => x.Id == updateCategory).SingleOrDefault();
                                    Console.WriteLine("1. Change Category");
                                    Console.WriteLine("2. Change Subcategory");
                                    Console.WriteLine("B to Back");
                                    string catOption = Console.ReadLine()!;
                                    int intcatOption = 0;
                                    if (BackOption(catOption))
                                        break;

                                    if (int.TryParse(catOption, out intcatOption))
                                    {
                                        if (intcatOption == 1)
                                        {
                                            Console.Write($"Which Category do u want to change to? Chose ID: ");
                                            string categoryChange = Console.ReadLine()!;
                                            int intcategoryChange = 0;

                                            if (int.TryParse(categoryChange, out intcategoryChange))
                                            {
                                                var updateToNewCategory = db.ProductCategory.Where(x => x.Id == intcategoryChange)
                                                    .Select(x => new { x.Id, x.ProductCategoryName }).SingleOrDefault();

                                                Console.WriteLine("Which Subcategory in that Category do you want to change to? Chose ID: ");
                                                string subcatCheck = Console.ReadLine()!;
                                                int intsubcatCheck = 0;

                                                if (int.TryParse(subcatCheck, out intsubcatCheck))
                                                {
                                                    var updateToNewSubcategory = db.ProductSubcategory.Where(x => x.Id == intsubcatCheck)
                                                    .Select(x => new { x.Id, x.ProductSubcategoryName }).SingleOrDefault();

                                                    var controllForNull = db.ProductSubcategory.Where(x => x.Id == intsubcatCheck && x.ProductCategoryId == intcategoryChange).SingleOrDefault();


                                                    if (updateToNewSubcategory != null && controllForNull != null)
                                                    {
                                                        productToAlter!.ProductCategoryId = updateToNewCategory!.Id;
                                                        productToAlter!.ProductSubcategoryId = updateToNewSubcategory.Id;
                                                        RunProgram.ChangeColor("Product Categories have been succefully changed", "Green");
                                                        Thread.Sleep(1000);
                                                        db.SaveChanges();
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                        else if (intcatOption == 2)
                                        {
                                            var subcategoryUpdate = db.Shop.Where(x => x.Id == updateCategory)
                                                .Select(x => new { x.ProductCategoryId, x.Id }).SingleOrDefault();

                                            Console.Write($"Which Subcategory do u want to change to?: ");
                                            string subCategoryChange = Console.ReadLine()!;
                                            int intsubCategoryChange = 0;

                                            if (int.TryParse(subCategoryChange, out intsubCategoryChange))
                                            {
                                                var updateToNewSubcategory = db.ProductSubcategory.Where(x => x.Id == intsubCategoryChange)
                                                    .Select(x => new { x.Id, x.ProductSubcategoryName, x.ProductCategoryId }).SingleOrDefault();

                                                if (updateToNewSubcategory != null && updateToNewSubcategory.ProductCategoryId == subcategoryUpdate!.ProductCategoryId)
                                                {
                                                    productToAlter!.ProductSubcategoryId = updateToNewSubcategory!.Id;
                                                    RunProgram.ChangeColor("Product Subcategory have been succefully changed", "Green");
                                                    Thread.Sleep(1000);
                                                    db.SaveChanges();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            break;

                        case 9:
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
                                            Console.Clear();
                                            var allCategory = db.ProductCategory.Select(x => new { x.Id, x.ProductCategoryName })
                                                .OrderBy(x => x.Id)
                                                .ToList();

                                            Console.WriteLine("B to back");
                                            Console.WriteLine("Which do you want to Delete?");
                                            foreach (var cate in allCategory)
                                            {
                                                Console.WriteLine($"{cate.Id}. {cate.ProductCategoryName}");
                                            }
                                            string inputDelete = Console.ReadLine()!;
                                            int intInputDelete = 0;

                                            if (inputDelete.ToLower() == "b")
                                                break;

                                            if (int.TryParse(inputDelete, out intInputDelete) && intInputDelete > 0)
                                            {
                                                var selectedCategory = allCategory[intInputDelete - 1];

                                                var cateToDelete = db.ProductCategory.Where(x => x.Id == selectedCategory.Id).SingleOrDefault();
                                                db.ProductCategory.Remove(cateToDelete!);
                                                db.SaveChanges();

                                                RunProgram.ChangeColor("Category have been succefully Deleted", "Green");
                                                Thread.Sleep(1500);
                                            }
                                            else
                                                RunProgram.ErrorMessage();
                                        }
                                    }
                                    else if (intDeleteCategory == 2)
                                    {
                                        while (true)
                                        {
                                            int counter = 1;
                                            Console.Clear();
                                            var allSubcategory = db.ProductSubcategory.Select(x => new { x.Id, x.ProductSubcategoryName })
                                                .OrderBy(x => x.Id).ToList();

                                            Console.WriteLine("B to back");
                                            Console.WriteLine("Which do you want to Delete?");
                                            foreach (var cate in allSubcategory)
                                            {
                                                Console.WriteLine($"{counter}. {cate.ProductSubcategoryName}");
                                                counter++;
                                            }
                                            string inputDelete = Console.ReadLine()!;
                                            int intInputDelete = 0;

                                            if (inputDelete.ToLower() == "b")
                                                break;

                                            if (int.TryParse(inputDelete, out intInputDelete) && intInputDelete > 0 && intInputDelete <= allSubcategory.Count)
                                            {
                                                var selectedCategory = allSubcategory[intInputDelete - 1];

                                                var cateToDelete = db.ProductSubcategory.Where(x => x.Id == selectedCategory.Id).SingleOrDefault();
                                                db.ProductSubcategory.Remove(cateToDelete!);
                                                db.SaveChanges();

                                                RunProgram.ChangeColor("Subcategory have been succefully Deleted", "Green");
                                                Thread.Sleep(1500);
                                            }
                                            else
                                                RunProgram.ErrorMessage();
                                        }
                                    }
                                }
                                else
                                    RunProgram.ErrorMessage();
                            }
                            //db.SaveChanges();
                            break;

                        case 10:
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
                                            RunProgram.ErrorMessage();
                                    }
                                }
                            }

                            break;

                        case 11:
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
                                        bool runningProgram = true;
                                        while (runningProgram)
                                        {
                                            Console.Clear();
                                            Console.Write("What do you want to do with ");
                                            RunProgram.ChangeColor("Customer:", "Green");
                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                            Console.WriteLine($"\nName: {customer!.Name}   Lastname: {customer.LastName}   Age: {customer.Age}   Username: {customer.UserName}   isAdmin: {customer.IsAdmin}   Logins: {customer.Logins}\n");
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

                                                            RunProgram.ChangeColor("\nCustomer is now deleted from Database", "Red");
                                                            Thread.Sleep(1500);
                                                            db.SaveChanges();
                                                            runningProgram = false;
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

                                                            RunProgram.ChangeColor("Name updated", "Green");
                                                            Thread.Sleep(1500);
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

                                                            RunProgram.ChangeColor("Lastname updated", "Green");
                                                            Thread.Sleep(1500);
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
                                                                RunProgram.ChangeColor("Age updated", "Green");
                                                                Thread.Sleep(1500);
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

                                                            RunProgram.ChangeColor("Username updated", "Green");
                                                            Thread.Sleep(1500);
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

                                                            customer.Password = BC.EnhancedHashPassword(passwordUpdate, 14);

                                                            RunProgram.ChangeColor("Password updated", "Green");
                                                            Thread.Sleep(1500);
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

                                                            RunProgram.ChangeColor("isAdmin updated", "Green");
                                                            Thread.Sleep(1500);
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

                        case 12:
                            Console.Clear();

                            var allOrders = db.Order.Include(a => a.Customer)
                                .Include(a => a.Products)
                                .GroupBy(x => x.Customer!.UserName);

                            if (allOrders.Count() == 0)
                                Console.WriteLine("The orderlist is empty at the moment");
                            else
                            {
                                Console.Clear();
                                foreach (var orders in allOrders)
                                {
                                    Console.Write($"UserName: ");
                                    RunProgram.ChangeColor($"{orders.Key}", "DarkGreen");
                                    Console.WriteLine("\n-------------------------------");
                                    foreach (var item in orders)
                                    {
                                        Console.Write($"OrderID: ");
                                        RunProgram.ChangeColor($"{item.Id}", "DarkCyan");
                                        Console.Write($" Order created: {item.Date}  Payment: {item.PaymentChoice!.PadRight(17)}\tCollected: {item.Shipping}");
                                        Console.WriteLine("Products:");
                                        foreach (var product in item.Products!)
                                        {
                                            Console.WriteLine($"Name: {product.Name!.PadRight(48)} Amount: {product.Amount} - Price: {product.Price:C}");

                                        }
                                        Console.WriteLine($"------ Shipping: {item.ShippingFee:C} ------ Taxes: {Convert.ToInt32((item.TotalAmountPrice - item.ShippingFee) * 0.25):C} ------ Total Price: {item.TotalAmountPrice:C} ------\n");
                                    }
                                    Console.WriteLine();
                                }
                            }

                            Console.WriteLine("");
                            Console.ReadKey();
                            break;

                        case 13:
                            ChangeTop3();
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

    private static void Categorylist()
    {
        using (var db = new MyDbContext())
        {
            Console.Clear();

            var categoryInfo = db.ProductCategory.Select(x => new { x.ProductCategoryName, x.Id }).ToList();

            var subcategoryInfo = db.ProductSubcategory.Select(x => new { x.Id, x.ProductSubcategoryName, x.ProductCategoryId })
                .ToList();


            // Denna loopen skriver ut listan så man ser vad man kan köpa
            foreach (var name in categoryInfo)
            {
                RunProgram.ChangeColor($"Category: {name.ProductCategoryName} - ID: {name.Id}", "DarkCyan");
                Console.WriteLine("");
                foreach (var subcategoryName in subcategoryInfo)
                {
                    if (subcategoryName.ProductCategoryId == name.Id)
                    {
                        RunProgram.ChangeColor($"  Subcategory: {subcategoryName.ProductSubcategoryName} - ID: {subcategoryName.Id}", "DarkGreen");
                        Console.WriteLine("\n----------------------");

                        var productsInfo = db.Shop.Where(x => x.ProductSubcategoryId == subcategoryName.Id).ToList();

                        foreach (var product in productsInfo)
                        {
                            Console.Write($"{product.Id}. Name: {product.Name}  -  in stock: ");

                            if (product.Quantity > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($"{product.Quantity}");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{product.Quantity}");
                            }
                            Console.ResetColor();
                            Console.Write(" -  Price: ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"{product.Price:C}");
                            Console.ResetColor();
                        }
                        Console.WriteLine("---------------------------");
                    }

                }
                Console.WriteLine();
            }
        }
    }

    private static void ChangeTop3()
    {
        while (true)
        {
            using (var db = new MyDbContext())
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
                    var allCategories = db.Shop.GroupBy(x => x.ProductSubcategoryId).ToList();

                    List<(int? Subcategory, int? totalSold)> topCategories = new List<(int?, int?)>(); // Tillfälligt skapad Lista för att sortera ut

                    foreach (var key in allCategories)
                    {
                        int? soldItem = 0;
                        foreach (var product in key)
                        {
                            soldItem += product.Sold;
                        }
                        topCategories.Add((key.Key, soldItem));
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

                        var toUpdate = db.Shop.Where(x => x.ProductSubcategoryId == top.Subcategory).ToList();

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
        }
    }

    public static void UpdatingTop3()
    {
        using (var db = new MyDbContext())
        {
            var resetTop = db.Shop.OrderByDescending(x => x.IsActiveCategory)
                .ToList();

            var topSellers = db.Shop
                .OrderByDescending(x => x.Sold)
                .Take(3).ToList();

            int activeCategory = 0;
            foreach (var sell in resetTop)
            {
                if (sell.IsActiveCategory == 1)
                    activeCategory = 1;
                else if (sell.IsActiveCategory == 2)
                    activeCategory = 2;
            }

            if (activeCategory == 1)
            {

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
            }
            else if (activeCategory == 2)
            {
                // Här kommer logiken för att få fram vilken kategori som har mest sålda produkter
                var allCategories = db.Shop.GroupBy(x => x.ProductSubcategoryId).ToList();


                List<(int? Subcategory, int? totalSold)> topCategories = new List<(int?, int?)>(); // Tillfälligt skapad Lista för att sortera ut

                foreach (var key in allCategories)
                {

                    int? soldItem = 0;
                    foreach (var product in key)
                    {
                        soldItem += product.Sold;
                    }
                    topCategories.Add((key.Key, soldItem));
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

                    var toUpdate = db.Shop.Where(x => x.ProductSubcategoryId == top.Subcategory).ToList();

                    foreach (var item in toUpdate)
                    {
                        item.IsActive = true;
                        item.IsActiveCategory = 2;
                    }
                }
                db.SaveChanges();
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
