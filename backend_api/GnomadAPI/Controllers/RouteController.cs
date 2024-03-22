/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 12/28/2022
*
* Purpose: Contains Tag Controllers. Probably temporary since tags are defined at launch.
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TravelCompanionAPI.Models;
using TravelCompanionAPI.Data;
using GnomadAPI.Models;
using System.Security.Claims;

namespace TravelCompanionAPI.Controllers
{
        /// <summary>
        /// Default route controller.
        /// </summary>
    [Route("routes")]
    [ApiController]
    [Authorize]
    public class RouteController : ControllerBase
    {
        //The repository obtained through dependency injection.
        private IRouteRepository _route_repo;

        public RouteController(IRouteRepository route_repo)
        {
            _route_repo = route_repo;
        }

        [HttpGet("get/{id}")]
        public JsonResult get(int id)
        {
            //Get the user for the id
            var identity = (User.Identity as ClaimsIdentity);
            User user = new User(identity);

            Route route = _route_repo.getById(id, user.Id);

            if (route == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(route));
        }

        [HttpGet("all")]
        public JsonResult getAll()
        {
            //Get the user and set id
            var identity = (User.Identity as ClaimsIdentity);
            User user = new User(identity);

            List<Route> routes = _route_repo.getAll(user.Id);

            return new JsonResult(Ok(routes));
        }

        [HttpPost("create")]
        public JsonResult create(Route route)
        {
            //Get the user and set id
            var identity = (User.Identity as ClaimsIdentity);
            User user = new User(identity);
            route.UserId = user.Id;

            _route_repo.add(route);

            return new JsonResult(Ok(route));
        }
    }
}
