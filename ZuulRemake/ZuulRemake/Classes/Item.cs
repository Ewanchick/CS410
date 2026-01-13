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
        private String description;
        private int weight;
        private String name;

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

        public void setDescription()
        {

        }

        public String getDescription()
        {
            return description;
        }

        public String getName()
        {
            return name;
        }

        public void setName()
        {

        }

        public int getWeight()
        {
            return weight;
        }

        public void setWeight()
        {

        }
    }
}
