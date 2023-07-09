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
    [Index(nameof(Code), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public string Description { get; set; }
        public string Vendor { get; set; }
        public string FullUrl { get; set; }
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
