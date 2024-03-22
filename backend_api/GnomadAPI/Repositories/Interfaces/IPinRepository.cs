/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Defines the default functions for dependency injection. Declares getById, getAll, and add.
*
************************************************************************************************/

using System.Collections.Generic;
using TravelCompanionAPI.Models;

namespace TravelCompanionAPI.Data
{
    public interface IPinRepository
    {
        //******************************************************************************
        //This class defines the default functions for dependency injection
        //******************************************************************************

        public Pin getById(int id);

        public List<Pin> getAll();

        public bool add(Pin data);

        public bool delete(int pinId);

        public bool contains(Pin data);
        
        public List<Pin> getAllByUser(int uid);

        public List<Pin> getAllInArea(double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0);
    
        public List<Pin> getAllByTag(List<int> tags, double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0);

        public List<Pin> getAllByAddress(string address);

        public List<Pin> getByName(string searchString);

        public List<Pin> getByCity(string searchString);

        public double getAverageVote(int pinid);

        public List<Pin> globalSearch(string searchString);

        public List<Pin> getStickers(int uid, double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0);

        public bool autoRemove(int pinId);
    }
}
