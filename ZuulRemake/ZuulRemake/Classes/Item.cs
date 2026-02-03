using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    public class Item(string name, string description, int weight, int? buff)
    {
        public string Description { get; set; } = description;
        public int Weight { get; set; } = weight;
        public string Name { get; set; } = name;
        public int? StatIncrease { get; set; } = buff;


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

            switch (Name)
            {
                case "Potion":
                    ReturnString += $"Health Increase: {StatIncrease}";
                    break;
                default:
                    ReturnString += $"Buff: {StatIncrease}";
                    break;
            }

            return ReturnString;
        }
    }
}
