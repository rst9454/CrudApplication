using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Models.Account
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long? Mobile { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
