/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 1/27/2023
*
* Purpose: Defines the default functions for dependency injection. Declares getByH3Id, getAll
*
************************************************************************************************/

using System.Collections.Generic;
using TravelCompanionAPI.Models;

namespace TravelCompanionAPI.Data
{
    public interface ICellularRepository
    {
        //******************************************************************************
        //This class defines the default functions for dependency injection
        //Declares getByH3Id, getAll
        //******************************************************************************

        public void SaveToDatabase(string state);

        public List<string> getAllH3(string state);

        public List<float> getAllCoordsThreaded(int max_pass, float latMin, float lngMin, float latMax, float lngMax);
        
        public List<float> getAllCoordsSingle(int max_pass, int pass, float latMin, float lngMin, float latMax, float lngMax);
    }
}
