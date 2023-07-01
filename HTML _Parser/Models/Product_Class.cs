using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HTML__Parser.Models
{
    [Index(nameof(ProductName), IsUnique = true)]
    public class Product
    {
        [Key] 
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool Avaliable { get; set; }
        public string Discription { get; set; }
        public string Value { get; set; }

        public string FullUrl { get; set; }


        public DateTime FirstCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }

}
