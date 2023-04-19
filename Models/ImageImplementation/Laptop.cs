using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Models.ImageImplementation
{
    public class Laptop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Description { get; set; }
        public string Path { get; set; }
        [NotMapped]
        [Display(Name = "Choose Image")]
        public IFormFile ImagePath { get; set; }
    }
}
