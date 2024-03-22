/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Temp file to test user authentication.
*
************************************************************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelCompanionAPI.Models;

namespace TravelCompanionAPI.Controllers;

/// <summary>
/// The default route controller.
/// </summary>
[Route("")]
[ApiController]
[Authorize]
public class HomeController : Controller
{
    /// <summary>
    /// Test endpoint that returns the name of the logged in user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public JsonResult Index()
    {
        var currentUser = User.Identity.Name.Split(' ');
        var first_name = currentUser[0];
        var last_name = currentUser[1];

        User user = new User("", "", first_name, last_name);
        return new JsonResult(Ok(user));
    }
}
