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
        private String description { get; set; }
        private int weight { get; set; }
        private String name { get; set; }

        /**
         * Constructor for objects of class Item
         */
        public Item(String name, String description, int weight)
        {
            this.name = name;
            this.description = description;
            this.weight = weight;
        }

        public String toString()
        {
            return "name: " + name + "\ndescription: " + description + "\nweight: " + weight;
        }

        
    }
}
