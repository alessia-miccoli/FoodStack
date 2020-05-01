using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStack.Models
{
    public class Food
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int QuantityAvailable { get; set; }
        public int QuantityBooked { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
