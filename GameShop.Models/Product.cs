using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Developer { get; set; } // The name of the company or individual that developed the game.
        [Required]
        public string Publisher { get; set; } // The name of the company that published the game.
        public string ImageUrl { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required, Range(1,10000)]
        public double ListPrice { get; set; }
        [Required, Range(1, 10000)]
        public double Price { get; set; }
        [Required, Range(1, 10000)]
        public double Price50 { get; set; }
        [Required, Range(1, 10000)]
        public double Price100 { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        [Required]
        public int PlatformId { get; set; }
        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; }
    }
}
