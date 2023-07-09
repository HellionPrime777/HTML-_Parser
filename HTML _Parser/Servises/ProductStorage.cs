using HTML__Parser.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML__Parser.Servises
{
    public class ProductStorage
    {
        private ProductContext _context;
        public ProductStorage(ProductContext context)
        {
            _context = context;
        }

        public Product? FindByCode(string code)
        {
            return _context.Products.FirstOrDefault(x => x.Code == code);
        }

        public void Update(List<ProductList> products)
        {
            products.ForEach(p =>
            {
                var dbProduct = FindByCode(p.Number);
                if (dbProduct == null)
                {
                    dbProduct = new Product();
                    dbProduct.CreatedAt = DateTime.Now;
                    _context.Add(dbProduct);
                }
                dbProduct.Code = p.Number;
                dbProduct.Title = p.Title;
                dbProduct.Vendor = p.Vendor;
                dbProduct.Description = p.Details;
                dbProduct.Price = Decimal.Parse(p.Price, CultureInfo.InvariantCulture);
                dbProduct.UpdatedAt = DateTime.Now;
                dbProduct.Available = p.Status == "In stock";
                dbProduct.FullUrl = p.FullUrl;
                dbProduct.ImageUrl = p.ImageUrl;
            });
            _context.SaveChanges();
        }
    }
}
