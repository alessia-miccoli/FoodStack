using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodStack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookedFoodController : ControllerBase
    {
        private ApplicationContext _context;


        public BookedFoodController(ApplicationContext context)
        {
            _context = context;
        }


        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]FoodBooked foodBooked)
        {


            //check if food exists and there is quantity available
            var food = _context
                .Foods
                .FirstOrDefault(f => f.Id == foodBooked.FoodId);
            if (food == null)
            {
                return StatusCode(404, "Food not found");
            }

            var remainingFoodQuantity = food.QuantityAvailable - food.QuantityBooked;
            if (remainingFoodQuantity < foodBooked.QuantityBooked)
            {
                return StatusCode(500,
                    $"Cannot book {foodBooked.QuantityBooked} of {food.Name}, " +
                    $"{remainingFoodQuantity} is available");
            }

            var meal = _context
                .Meals
                .Include(m => m.FoodsBooked)
                .FirstOrDefault(m => m.Id == id);

            var existingBookedFood = meal.FoodsBooked.FirstOrDefault(f => f.FoodId == foodBooked.FoodId);

            if(existingBookedFood!= null)
            {
                existingBookedFood.QuantityBooked += foodBooked.QuantityBooked;
                _context.FoodsBooked.Update(existingBookedFood);
            }
            else
            {
                meal.FoodsBooked.Add(foodBooked);
            }

            food.QuantityBooked += foodBooked.QuantityBooked;

            _context.Update(food);
            _context.Update(meal);
            _context.SaveChanges();

            return StatusCode(200, "Food added to meal");
        }
    }
}