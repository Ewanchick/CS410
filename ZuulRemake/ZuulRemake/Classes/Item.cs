using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    internal class Item(string name, string description, int weight)
    {
        public string Description { get; set; } = description;
        public int Weight { get; set; } = weight;
        public string Name { get; set; } = name;
        public int StatIncrease { get; set; } = 0;


        /**
         * Returns a detailed description of the Item.
         * If Item is a health potion, displays health increase;
         * otherwise, displays level increase.
         */
        public override string ToString()
        {
            string ReturnString;
            ReturnString = $"{Name}\n" +
                   $"{Description}\n" +
                   $"Weight: {Weight}\n";

            if (Name == "Potion")
            {
                ReturnString += $"Health Increase: {StatIncrease}";
            }
            else
            {
                ReturnString += $"Buff: {StatIncrease}";
            }

            return ReturnString;
        }
    }
}
