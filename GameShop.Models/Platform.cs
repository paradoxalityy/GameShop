using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Platform"), MaxLength(50)]
        public string Name { get; set; }
    }
}
