/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Class for User object. Contains definition of User object for use elsewhere.
*
************************************************************************************************/


using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TravelCompanionAPI.Data;
using TravelCompanionAPI.Extras;

namespace TravelCompanionAPI.Models
{
    public class User
    {
        public User()
        { }

        public User(string email, string profile_photo_URL, string first_name, string last_name)
        {
            Email = email;
            ProfilePhotoURL = profile_photo_URL;
            FirstName = first_name;
            LastName = last_name;

            //Must come last (at least after email)
            UserRepository user_repo = new UserRepository();
            Id = user_repo.getId(this);
        }

        public User(ClaimsIdentity identity)
        {
            Email = identity.FindFirst(JwtRegisteredClaimNames.Email).Value;
            ProfilePhotoURL = "";

            var full_name = identity.Name;
            FirstName = parseFirstName(full_name);
            LastName = parseLastName(full_name);

            //Must come last (at least after email)
            UserRepository user_repo = new UserRepository();
            Id = user_repo.getId(this);
        }

        [BindNever] //User shouldn't be able to change Id
        public int Id { get; set; }
        public string Email { get; set; }
        public string ProfilePhotoURL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string parseFirstName(string full_name)
        {
            string first_name = full_name;

            if (!first_name.Contains(" ")) return first_name;

            var names = first_name.Split(' ');
            if (names.Length <= 1) return first_name;

            first_name = names[0];

            return first_name;
        }

        public static string parseLastName(string full_name)
        {
            string last_name = "";

            if (!full_name.Contains(" ")) return last_name;

            var names = full_name.Split(' ');
            if (names.Length <= 1) return last_name;

            last_name = names[1];

            return last_name;
        }

        public void print()
        {
            Console.WriteLine(
                "id: {0}\nemail: {1}\nprofile photo url: {2}\nfirst name: {3}\nlast name: {4}"
                , Id, Email, ProfilePhotoURL, FirstName, LastName);
        }
    }
}
