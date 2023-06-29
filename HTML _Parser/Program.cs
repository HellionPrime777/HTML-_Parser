using HTML__Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using OfficeOpenXml;


System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var baseUrl = "https://www.swansonvitamins.com";
var products = new List<Product>();

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
            var pageProducts = JsonSerializer.Deserialize<List<Product>>(json);
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
