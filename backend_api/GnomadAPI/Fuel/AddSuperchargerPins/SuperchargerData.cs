//Script to add supercharger pins to the database.
using System.Collections.Generic;


//Generic layout of JSON
public class SuperchargerData
{
    public int? id { get; set; }
    public string? locationId { get; set; } //Possible name
    public string? name { get; set; } //Possible name (used below)
    public string? status { get; set; } //Only use data if !strcmp(status, "OPEN")
    //public Dictionary<string, AddressData> address { get; set; }
    public AddressData address { get; set; } //Useful. Address and only use data if !strcmp(address["country"], "USA".
    public LatitudeAndLongitude gps { get; set; } //Useful.
    public string? dateOpened { get; set; }
    public int? stallCount { get; set; } //Possibly nice, not for now
    public bool? counted { get; set; }
    public int? elevationMeters { get; set; }
    public int? powerKilowatt { get; set; }
    public bool? solarCanopy { get; set; }
    public bool? battery { get; set; }
    public int? statusDays { get; set; }
    public bool? urlDiscuss { get; set; }
}
//Layout of gps data member in JSON
public class LatitudeAndLongitude
{
    public double? latitude { get; set; }
    public double? longitude { get; set; }
}
//Layout of address data member in JSON
public class AddressData
{
    public string? street { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? zip { get; set; }
    public int? countryId { get; set; }
    public string? country { get; set; }
    public int? regionId { get; set; }
    public string? region { get; set; }
}