/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Class definition for tags
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace TravelCompanionAPI.Models
{
    public class Tag
    {
        public Tag()
        { }

        public Tag(string type)
        {
            Id = -1;
            Type = type;
        }
        [BindNever] //User shouldn't be able to change Id
        public int Id { get; set; }
        public string Type { get; set; }

        public void print()
        {
            Console.WriteLine(
                "id: {0}\ntype: {1}", Id, Type);
        }
    }
}
