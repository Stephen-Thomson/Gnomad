//Script to add supercharger pins to the database.
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TravelCompanionAPI.Data;
using TravelCompanionAPI.Models;

/// <summary>
/// Puts the supercharger pins into the database
/// </summary>
/// <returns>
/// Adds all of the supercharger pins to the database
///</returns>
namespace TravelCompanionAPI.Fuel
{
    static class AddingSuperchargerData
    {
        //Adds the relevent data to the database.
        public static void AddSuperchargers(IPinRepository pin_repo)
        {
            //Get connection string and set up database done in pin_repo (of type PinTableModifier)
            //File I/O
            string path = Path.Join(Directory.GetCurrentDirectory(), "\\Fuel\\AddSuperchargerPins\\SuperchargerData.txt");

            if (File.Exists(path)) //Only run if file is found (should be found)
            {
                foreach (SuperchargerData data in JsonSerializer.Deserialize<List<SuperchargerData>>(File.ReadAllText(path)))
                {
                    if (data.address.country.Equals("USA") && data.status.Equals("OPEN")) //If an open us supercharger
                    {
                        Pin pin = new Pin();
                        pin.UserId = 0;
                        pin.Longitude = data.gps.longitude ?? 0;
                        pin.Latitude = data.gps.latitude ?? 0;
                        pin.Title = data.name + " Supercharger";
                        pin.Street = data.address.street;
                        pin.Tags.Add((int)TagValues.tags.TeslaSupercharger);

                        pin_repo.add(pin);
                    }
                }
            }
        }
    }
}