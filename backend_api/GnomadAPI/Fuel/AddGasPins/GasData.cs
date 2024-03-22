//Script to add gas pins to the database.
using System.Collections.Generic;


//Generic layout of JSON
public class GasData
{
    public string? type { get; set; } //Always Feature? Doesn't seem useful.
    public Properties properties { get; set; } //Useful, has type of gas
    public Geometry geometry { get; set; } //Useful, lat and long
    public string? id { get; set; }
}
//Layout of geometry data member in JSON
public class Geometry
{
    public string? type { get; set; }
    public List<double> coordinates { get; set; } //Stored latitude, longitude
}
//Layout of coordinates for geometry
/*public class Coordinates
{
    public double? latitude { get; set; }
    public double? longitude { get; set; }
}*/
//Layout of properties data member in JSON
public class Properties
{
    public string? pid { get; set;}
    public string? addr_city { get; set;} //Potentially useful
    public string? addr_housenumber { get; set;} //Useful
    public string? addr_postcode { get; set;}
    public string? addr_state { get; set;} //Potentially useful
    public string? addr_street { get; set;} //Useful
    public string? amenity { get; set;} //Should be fuel
    public string? brand { get; set;} //Potentialy useful
    public string? brand_wikidata { get; set;}
    public string? brand_wikipedia { get; set;}
    public string? census_population { get; set; }
    public string? cuisine { get; set; }
    public string? ele { get; set; }
    public string? fuel_biodiesel { get; set; } //Useful
    public string? fuel_diesel { get; set;} //Useful
    public string? fuel_gasoline { get; set;} //Useful
    public string? fuel_octane_85 { get; set;}
    public string? fuel_octane_87 { get; set;}
    public string? fuel_octane_91 { get; set;}
    public string? fuel_e10 { get; set;} //Ethanol 10
    public string? fuel_lpg { get; set;} //liquefied petroleum
    public string? fuel_LH2 { get; set;} //liqid hydrogen
    public string? gnis_Class { get; set; }
    public string? gnis_County { get; set; }
    public string? gnis_County_num { get; set; }
    public string? gnis_ST_alpha { get; set; }
    public string? gnis_id { get; set; }
    public string? gnis_ST_num { get; set; }
    public string? import_uuid { get; set; }
    public string? name { get; set;} //Useful
    public string? opening_hours { get; set;} //Potentially useful
    public string? phone { get; set;} //Potentially useful
    public string? self_service { get; set;}
    public string? website { get; set; } //Potentially useful
    public string? payment_cards { get; set; }
    public string? payment_cash { get; set; }
    public string? payment_credit_cards { get; set; }
    public string? source { get; set; }
    public string? source_ref { get; set; }
    public string? population { get; set; }
}

/*
amenity=fuel
brand=CONCH

operator=BigOil Inc.

name=Classic
tenant=John Doe
opening_hours=Mo-Fr 05:30-23:00; Sa 07:00-21:00; Su off
payment:cash=yes
payment:mastercard=yes
payment:visa=yes
payment:diners_club=no
payment:american_express=yes
payment:maestro=yes
payment:dkv=yes
payment:uta=yes
fuel:diesel=yes
fuel:octane_91=yes
fuel:octane_95=yes
fuel:octane_98=yes
fuel:e10=yes
fuel:lpg=yes
fuel:LH2=yes
*/