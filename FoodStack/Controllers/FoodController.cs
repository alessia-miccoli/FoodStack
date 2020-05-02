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

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}