using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStack.Models
{
    public class FoodBooked
    {
        public Guid Id { get; set; }
        public Guid FoodId { get; set; }
        public int QuantityBooked { get; set; }
    }
}
