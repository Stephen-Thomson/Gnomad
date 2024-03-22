/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Class definition for pins
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelCompanionAPI.Models
{
    //For both pins and stickers (also pins)
    public class Pin
    {
        public Pin()
        { 
            Tags = new List<int>();
        }

        public Pin(int longitude, int latitude, string title)
        {
            Id = -1;
            Longitude = longitude;
            Latitude = latitude;
            Title = title;
            Tags = new List<int>();
        }

        [BindNever] //User shouldn't be able to change Id
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public List<int> Tags { get; set; }

        public void print()
        {
            Console.WriteLine(
                "id: {0}\nlongitude: {1}\nlatitude name: {2}\ntitle: {3}\nstreet: {4}\nTag: {5}", Id, Longitude, Latitude, Title, Street, Tags.First());
        }
    }
}
