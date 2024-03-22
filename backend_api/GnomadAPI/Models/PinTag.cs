/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Class definition for pintags
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelCompanionAPI.Data;

namespace TravelCompanionAPI.Models
{
    public class PinTag
    {
        public PinTag()
        { }

        public PinTag(int pid, int tid)
        {
            PinId = pid;
            TagId = tid;
        }
        public int PinId { get; set; }
        public int TagId { get; set; }

        public void print()
        {
            Console.WriteLine(
                "pin id: {0}\ntag id: {1}", PinId, TagId);
        }
    }
}
