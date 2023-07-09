//using HTML__Parser;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using OfficeOpenXml;
//using HTML__Parser.Models;
//using System.Globalization;


// See https://aka.ms/new-console-template for more information
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using HTML__Parser.Models;
using HTML__Parser.Servises;
using HTML__Parser;

using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Irony.Parsing;
using SwansonParser2023.Services;

Console.WriteLine("Hello, World!");

/*
 * https://media.swansonvitamins.com/images/items/master/SW1876.jpg
https://www.swansonvitamins.com/ncat1/Vitamins+and+Supplements/ncat2/Multivitamins/ncat3/Multivitamins+with+Iron/q
https://www.swansonvitamins.com/ncat1/Vitamins+and+Supplements/ncat2/Multivitamins/ncat3/Multivitamins+with+Iron/q?page=2
https://www.swansonvitamins.com/ncat1/Vitamins+and+Supplements/ncat2/Multivitamins/ncat3/Multivitamins+with+Iron/q?page=3
 * 
 * https://www.swansonvitamins.com/q
 */

//var html = File.ReadAllText("page.html");

var catalogUrl = "https://www.swansonvitamins.com/q";

var parser = new CatalogParser() { Page = 1 };

//IProductWriter writer = new ExcelProductWriter();
//writer.SaveAs("products123.xlsx", products);

using (var db = new ProductContext())
{
    var storage = new ProductStorage(db);
    while (parser.HasMore)
    {
        var products = parser.Parse(catalogUrl);
        storage.Update(products);
        Thread.Sleep(3000);
    }
}




//System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//var baseUrl = "https://www.swansonvitamins.com";
//var products = new List<ProductList>();

//for (int page = 1; page <= 10; page++)
//{
//    var url = $"{baseUrl}/ncat1/Vitamins+and+Supplements/ncat2/Multivitamins/ncat3/Multivitamins+with+Iron/q?page={page}";

//    using (var client = new HttpClient())
//    {
//        var html = client.GetStringAsync(url).Result;

//        var pattern = @"adobeRecords"":(.+),""topProduct";
//        var matches = Regex.Matches(html, pattern);
//        if (matches.Count > 0)
//        {
//            var json = matches[0].Groups[1].Value;
//            var pageProducts = JsonSerializer.Deserialize<List<ProductList>>(json);
//            products.AddRange(pageProducts);
//        }
//    }
//}
//foreach (var product in products)
//{
//    Console.WriteLine($"{product.Number} {product.Title} {product.Vendor} {product.Price}");
//}

//var excelPackage = new ExcelPackage();
//var worksheet = excelPackage.Workbook.Worksheets.Add("Products");

//worksheet.Cells.LoadFromCollection(products, true);

//var fileInfo = new FileInfo("products.xlsx");
//excelPackage.SaveAs(fileInfo);

//Console.WriteLine();
//Console.WriteLine("----------------------------------------------------------------------------------------");
//Console.WriteLine($"download success {fileInfo.FullName}.");
//Console.WriteLine("----------------------------------------------------------------------------------------");
//Console.WriteLine();

//using (var db = new ProductContext())
//{
//    products.ForEach(p =>
//    {
//        var dbProduct = db.Products.FirstOrDefault(x => x.ProductName == p.Number);
//        if (dbProduct == null)
//        {
//            dbProduct = new Product();
//            dbProduct.FirstCreate = DateTime.Now;
//            db.Add(dbProduct);
//        }
//        dbProduct.Title = p.Title;
//        dbProduct.Value = p.Vendor;
//        dbProduct.Discription = p.Details;
//        dbProduct.ProductName = p.Number;
//        dbProduct.Price = Decimal.Parse(p.Price, CultureInfo.InvariantCulture);
//        dbProduct.FirstCreate = DateTime.Now;
//        dbProduct.FullUrl = p.Url;
//        dbProduct.ImageUrl = p.ImageUrl;
//        dbProduct.Avaliable = p.Status == "In stock";

//    });
//    db.SaveChanges(); 
//}