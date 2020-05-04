using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class FoodController : ControllerBase
    {
        private readonly ApplicationContext _context;


        public FoodController(ApplicationContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<ICollection<Food>> Get([FromHeader] Guid userId)
        {
            var boardId = _context
                 .Users
                 .Include(u => u.Boards)
                 .FirstOrDefault(u => u.Id == userId)
                 .Boards
                 .FirstOrDefault()
                 .Id;

            var foodList = _context
                .Foods
                .Where(f => f.Board.Id == boardId)
                .OrderBy(f=>f.ExpirationDate);

            return StatusCode(200, foodList);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Food food, [FromHeader] Guid userId)
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
                .Include(b => b.Food)
                .FirstOrDefault(b=>b.Id == boardId)
                .Food
                .Add(food); 
           

            await _context.SaveChangesAsync();

            return StatusCode(201, "Food Created");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromForm] IFormCollection values)
        {
            var foodToUpdate = _context.Foods.FirstOrDefault(f => f.Id == id);
            if (values.ContainsKey("ExpirationDate"))
            {
                if (DateTime.TryParse(values["ExpirationDate"], out DateTime parsedDate))
                {
                    foodToUpdate.ExpirationDate = parsedDate;
                }
                else
                {
                    return StatusCode(400, "Cannot parse Expiration Date");
                }
            }

            if (values.ContainsKey("Name"))
            {
                foodToUpdate.Name = values["Name"];
            }

            if (values.ContainsKey("QuantityAvailable"))
            {
                if (int.TryParse(values["QuantityAvailable"], out int quantityAvailable))
                {
                    foodToUpdate.QuantityAvailable = quantityAvailable;
                }
                else
                {
                    return StatusCode(400, "Cannot parse quantity available");
                }
            }

            if (values.ContainsKey("QuantityBooked"))
            {
                if (int.TryParse(values["QuantityBooked"], out int quantityBooked))
                {
                    foodToUpdate.QuantityBooked = quantityBooked;
                }
                else
                {
                    return StatusCode(400, "Cannot parse quantity booked");
                }
            }

            _context.Foods.Update(foodToUpdate);

            _context.SaveChanges();

            return StatusCode(200, "Food Updated");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var food = _context.Foods.FirstOrDefault(f => f.Id == id);
            _context.Foods.Remove(food);

            _context.Entry(food).State = EntityState.Deleted;
            _context.SaveChanges();


            return StatusCode(200, $"Food  deleted");
        }
    }
}