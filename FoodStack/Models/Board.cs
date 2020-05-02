﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStack.Models
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Name {get;set;}
        public virtual ICollection<Food> Food { get; set; }

        public virtual User User { get; set; }


        public Board()
        {
        }
    }
}
