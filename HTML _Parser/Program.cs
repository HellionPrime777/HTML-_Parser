using HTML__Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using HTML__Parser.Models;
using System.Globalization;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var baseUrl = "https://www.swansonvitamins.com";
var products = new List<ProductList>();

for (int page = 1; page <= 10; page++)
{
    var url = $"{baseUrl}/ncat1/Vitamins+and+Supplements/ncat2/Multivitamins/ncat3/Multivitamins+with+Iron/q?page={page}";

    using (var client = new HttpClient())
    {
        var html = client.GetStringAsync(url).Result;

        var pattern = @"adobeRecords"":(.+),""topProduct";
        var matches = Regex.Matches(html, pattern);
        if (matches.Count > 0)
        {
            var json = matches[0].Groups[1].Value;
            var pageProducts = JsonSerializer.Deserialize<List<ProductList>>(json);
            products.AddRange(pageProducts);
        }
    }
}
foreach (var product in products)
{
    Console.WriteLine($"{product.Number} {product.Title} {product.Vendor} {product.Price}");
}

var excelPackage = new ExcelPackage();
var worksheet = excelPackage.Workbook.Worksheets.Add("Products");

worksheet.Cells.LoadFromCollection(products, true);

var fileInfo = new FileInfo("products.xlsx");
excelPackage.SaveAs(fileInfo);

Console.WriteLine();
Console.WriteLine("----------------------------------------------------------------------------------------");
Console.WriteLine($"download success {fileInfo.FullName}.");
Console.WriteLine("----------------------------------------------------------------------------------------");
Console.WriteLine();

using (var db = new ProductContext())
{
    products.ForEach(p =>
    {
        var dbProduct = db.Products.FirstOrDefault(x => x.ProductName == p.Number);
        if (dbProduct == null)
        {
            dbProduct = new Product();
            dbProduct.FirstCreate = DateTime.Now;
            db.Add(dbProduct);
        }
        dbProduct.Title = p.Title;
        dbProduct.Value = p.Vendor;
        dbProduct.Discription = p.Details;
        dbProduct.ProductName = p.Number;
        dbProduct.Price = Decimal.Parse(p.Price, CultureInfo.InvariantCulture);
        dbProduct.FirstCreate = DateTime.Now;
        dbProduct.FullUrl = p.Url;
        //dbProduct.ImageUrl = p.ImageUrl;
        dbProduct.Avaliable = p.Status == "In stock";

    });
    db.SaveChanges(); 
}