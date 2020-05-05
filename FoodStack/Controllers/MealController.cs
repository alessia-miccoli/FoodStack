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


        // POST api/values
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
    }
}