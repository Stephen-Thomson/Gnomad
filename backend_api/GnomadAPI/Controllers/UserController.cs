/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Contains User Controllers.
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TravelCompanionAPI.Models;
using TravelCompanionAPI.Data;
using System.Security.Claims;


namespace TravelCompanionAPI.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        //Repo is the list of users in the database
        private IUserRepository _user_database;
        public UserController(IUserRepository repo)
        {
            _user_database = repo;
        }

        [HttpGet("get/{id}")]
        public JsonResult get(int id)
        {
            User user = _user_database.getById(id);

            if (user == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(user));
        }

        [HttpGet("all")]
        public JsonResult getAll()
        {
            // get a list of all the users from the database (could be very slow, don't actually use)
            List<User> users = _user_database.getAll();

            // return the list of users as a JSON object.
            return new JsonResult(Ok(users));
        }

        [HttpPost("login")]
        public JsonResult login()
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            if (!_user_database.contains(user))
            {
                _user_database.add(user);
            }

            // get the user id and return user
            user.Id = _user_database.getId(user);

            return new JsonResult(Ok(user));
        }
        [HttpGet("voted/{pinid}")]
        public JsonResult voted(int pinid)
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            return new JsonResult(Ok(_user_database.voted(user.Id, pinid)));
        }
        [HttpPost("review/{pinid}/{vote}")]
        public JsonResult review(int pinid, int vote)
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            int uid = -1;

            uid = _user_database.getId(user);

            _user_database.review(uid, pinid, vote);
            
            return new JsonResult(Ok("Success"));
        }

        //Gets the user's review value from the user_review table
        [HttpGet("getVote/{pinid}")]
        public JsonResult getVote(int pinid)
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            int uid = -1;

            uid = _user_database.getId(user);

            int uVote = _user_database.getVote(uid, pinid);

            return new JsonResult(Ok(uVote));
        }

        [HttpPost("cancelVote/{pinid}")]
        public JsonResult cancelVote(int pinid)
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            int uid = -1;

            uid = _user_database.getId(user);

            _user_database.cancelReview(uid, pinid);

            return new JsonResult(Ok("Success"));
        }
    }
}
