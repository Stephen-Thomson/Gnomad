//Script to add supercharger pins to the database.
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TravelCompanionAPI.Data;
using TravelCompanionAPI.Models;

/// <summary>
/// Puts the alternative fuel pins into the database
/// </summary>
/// <returns>
/// Adds all of the alternative fuel pins to the database
///</returns>
namespace TravelCompanionAPI.Fuel
{
    static class AddingAltFuelData
    {
        //Adds the relevent data to the database.
        public static void AddAltFuel(IPinRepository pin_repo)
        {
            //Get connection string and set up database done in pin_repo (of type PinTableModifier)
            //File I/O
            string path = Path.Join(Directory.GetCurrentDirectory(), "\\Fuel\\AddAltFuelPins\\AlternativeFuelingStations.txt");

            if (File.Exists(path)) //Only run if file is found (should be found)
            {
                foreach (AltFuelData data in JsonSerializer.Deserialize<List<AltFuelData>>(File.ReadAllText(path)))
                {
                    if (data.properties.country != null && data.properties.country.Equals("US") &&
                        data.properties.state != null && data.properties.state.Equals("OR") && !data.properties.fuel_type_code.Equals(null)) //If in Oregon
                    {
                        Pin pin = new Pin();
                        pin.UserId = 0;
                        pin.Longitude = data.properties.longitude;
                        pin.Latitude = data.properties.latitude;
                        pin.Title = data.properties.station_name;
                        pin.Street = data.properties.street_address;

                        //Gets the gas types; extras commented out.
                        switch(data.properties.fuel_type_code)
                        {
                            case "ELEC":
                                pin.Tags.Add((int)TagValues.tags.Electric);
                                break;
                            /*case "CNG":
                                break;
                            case "E85":
                                break;
                            case "BD":
                                break;*/
                            default:
                                break;
                        }

                        pin_repo.add(pin);
                    }
                }
            }
        }
    }
}