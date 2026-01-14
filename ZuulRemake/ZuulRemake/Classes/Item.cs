using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    internal class Item
    {
        // instance variables - replace the example below with your own
        public string Description { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }

        /**
         * Constructor for objects of class Item
         */
        public Item(string name, string description, int weight)
        {
            this.Name = name;
            this.Description = description;
            this.Weight = weight;
        }

        public string toString()
        {
            return "name: " + Name + "\ndescription: " + Description + "\nweight: " + Weight;
        }

        
    }
}
