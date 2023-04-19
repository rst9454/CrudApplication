using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Models.Cascade
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public State State { get; set; }
    }
}
