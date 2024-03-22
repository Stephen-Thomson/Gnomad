//Script to add alternative fuel pins to the database.
using Newtonsoft.Json;
using System.Collections.Generic;


//Generic layout of JSON
public class AltFuelData
{
    [JsonProperty("Properties")]
    public PropertiesData properties { get; set; } //Useful. Address and only use data if !strcmp(address["country"], "USA".

}
//Layout of properties member in JSON
public class PropertiesData
{
    public string access_code { get; set; }
    public string date_last_confirmed { get; set; }
    public string city {get; set;} //Use for address
    public string intersection_directions {get; set;}
    public string station_name {get; set;} //Use, name of pin
    public string station_phone {get; set;}
    public string state {get; set;} //Use, OR is Oregon
    public string street_address {get; set;} //Use, address
    public string zip {get; set;} //Use for address
    public string country {get; set;} //Use to clarify if US
    public string fuel_type_code {get; set;} //Use for fuel type -- CNG, ELEC, E85, BD, 
    public string e85_blender_pump {get; set;}
    public List<string> ev_connector_types {get; set;}
    public string ev_pricing {get; set;}
    public string ev_renewable_source {get; set;}
    public double longitude {get; set;} //Use for location
    public double latitude {get; set;} //Use for location
}