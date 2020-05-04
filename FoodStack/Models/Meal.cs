using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStack.Models
{
    public class Meal
    {
        public Guid Id { get; set; }
        public string MealName { get; set; }
        public string DishName { get; set; }
        public virtual ICollection<FoodBooked> FoodsBooked { get; set; }
        public DateTime Date { get; set; }
    }
}
