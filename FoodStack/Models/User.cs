using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStack.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string ProviderId { get; set; }
        public LoginProvider LoginProvider { get; set; }
        public string  Name { get; set; }
        public string ImgUrl { get; set; }
        public string Email { get; set; }
        public ICollection<Board> Boards { get; set;}


        public User()
        {
            Boards = new List<Board>
            {
                new Board($"{Name} Board")
            };
        } 
    }

    public enum LoginProvider { Google}
}
