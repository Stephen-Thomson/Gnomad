/************************************************************************************************
*
* Author: Andrew Rice
* Date: 3/6/2023
*
* Purpose: Defines the default functions for dependency injection. Declares getById, getAll, and add.
*
************************************************************************************************/

using System.Collections.Generic;
using GnomadAPI.Models;
using TravelCompanionAPI.Models;

//TODO: Some namespaces are TravelCompanion while others are Gnomad. Should change, but not important.
namespace TravelCompanionAPI.Data
{
    public interface IRouteRepository
    {
        //******************************************************************************
        //This class defines the default functions for dependency injection
        //******************************************************************************

        public Route getById(int id, int user_id);

        public List<Route> getAll(int userId);

        public bool add(Route data);
    }
}
