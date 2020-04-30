using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodStack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;


        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        //create user or login
        // POST api/user
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            User existingUser = _context
                .Users
                .FirstOrDefault
                (
                        u => (u.ProviderId == user.ProviderId) && (u.LoginProvider == user.LoginProvider)
                );

            if (existingUser!= null)
            {
                return StatusCode(200, user);
            }
            _context.Users.Add(user);

            _context.SaveChanges();

            return StatusCode(201, user);
        }
    }
}