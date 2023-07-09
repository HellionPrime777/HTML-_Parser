using HTML__Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML__Parser.Servises
{
    public interface IProductWriter
    {
        public void SaveAs(string file, List<ProductList> products);
        public void SaveAs(string file, List<Product> products);
    }
}
