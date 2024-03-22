using System.Collections.Generic;
using TravelCompanionAPI.Models;

namespace GnomadAPI.Models
{
    public class Route
    {
        public Route()
        {
            Pins = new List<Pin>();
        }
        //TODO: These are public in every model. Shouldn't they be private with getters/setters?
        public string Title {get; set;}
        public int Id {get; set;}
        public int UserId {get; set;}
        public List<Pin> Pins {get; set;}
    }
}
