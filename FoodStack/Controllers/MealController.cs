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
    public class MealController : ControllerBase
    {
        private ApplicationContext _context;


        public MealController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<ICollection<Meal>> Get([FromHeader]Guid userId)
        {
            var boardId = _context
                .Users
                .Include(u => u.Boards)
                .FirstOrDefault(u => u.Id == userId)
                .Boards
                .FirstOrDefault()
                .Id;

            var meals = _context
                .Boards
                .Include(b=>b.Meals)
                .FirstOrDefault(b => b.Id == boardId)
                .Meals;

            return StatusCode(200, meals);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Meal meal, [FromHeader] Guid userId)
        {
            var boardId = _context
                .Users
                .Include(u => u.Boards)
                .FirstOrDefault(u => u.Id == userId)
                .Boards
                .FirstOrDefault()
                .Id;

            _context
                .Boards
                .Include(b => b.Meals)
                .FirstOrDefault(b => b.Id == boardId)
                .Meals
                .Add(meal);


            await _context.SaveChangesAsync();

            return StatusCode(201, "Meal Created");
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromForm] IFormCollection values)
        {
            var mealToUpdate = _context.Meals.FirstOrDefault(m => m.Id == id);
            if(mealToUpdate == null)
            {
                return StatusCode(404, "Meal not found");
            }

            if (values.ContainsKey("Date"))
            {
                if (DateTime.TryParse(values["Date"], out DateTime parsedDate))
                {
                    mealToUpdate.Date = parsedDate;
                }
                else
                {
                    return StatusCode(400, "Cannot parse Date");
                }
            }

            if (values.ContainsKey("MealName"))
            {
                mealToUpdate.MealName = values["MealName"];
            }

            if (values.ContainsKey("DishName"))
            {
                mealToUpdate.DishName = values["DishName"];
            }

            _context.Update(mealToUpdate);

            _context.SaveChanges();

            return StatusCode(200, "Meal Updated");
        }
    }
}