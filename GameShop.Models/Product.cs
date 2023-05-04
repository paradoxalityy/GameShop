using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [ValidateNever]
        [DisplayName("Product Image")]
        public string ImageUrl { get; set; }
        [Required]
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        [Range(1, 10000)] 
        [DisplayName("List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        [DisplayName("Price for 1-50")]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        [DisplayName("Price for 51-100")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 10000)]
        [DisplayName("Price for 100+")]
        public double Price100 { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        [DisplayName("Platform")]
        public int PlatformId { get; set; }
        [ForeignKey(nameof(PlatformId))]
        [ValidateNever]
        public Platform Platform { get; set; }
    }
}
