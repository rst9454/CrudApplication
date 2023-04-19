using CrudApplication.Models;
using CrudApplication.Models.Account;
using CrudApplication.Models.Cascade;
using CrudApplication.Models.Excel;
using CrudApplication.Models.ImageImplementation;
using CrudApplication.Models.Stored_Proc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Wendor> Wendors { get; set; }
        public DbSet<Laptop> Laptops { get; set; }



    }
}
