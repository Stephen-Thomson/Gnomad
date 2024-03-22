/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 12/28/2022
*
* Purpose: Contains Pin Controllers.
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TravelCompanionAPI.Models;
using TravelCompanionAPI.Fuel;
using TravelCompanionAPI.Data;
using System.Security.Claims;

namespace TravelCompanionAPI.Controllers
{
    /// <summary>
    /// The default route controller.
    /// </summary>
    [Route("pins")]
    [ApiController]
    [Authorize]
    public class PinController : ControllerBase
    {
        //The repository obtained through dependency injection.
        private IPinRepository _pin_repo;
        private IUserRepository _user_repo;

        /// <summary>
        /// Constructor that takes in repo through dependecy injection
        /// </summary>
        /// <returns>
        /// Sets repository to PinTableModifier (defined in setup.cs)
        ///</returns>
        public PinController(IPinRepository pin_repo, IUserRepository user_repo)
        {
            _pin_repo = pin_repo;
            _user_repo = user_repo;
        }

        /// <summary>
        /// Gets a pin based on the pin's id.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of NotFound() if it's not found or 
        ///Ok(pin) with the pin found.
        ///</returns>
        [HttpGet("get/{id}")]
        public JsonResult get(int id)
        {

            Pin pin = _pin_repo.getById(id);

            if (pin == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pin));
        }

        /// <summary>
        /// Gets all of the pins in the database.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of Ok(pins) where pins is a list of every pin.
        ///</returns>
        [HttpGet("all")]
        public JsonResult getAll()
        {
            List<Pin> pins = _pin_repo.getAll();

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets all of the pins in the specified area, defaulting to OIT
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of NotFound() if no pins, or Ok(pins) if there are pins.
        ///</returns>
        [HttpGet("getAllInArea")]
        public JsonResult getAllInArea(double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0)
        {
            List<Pin> pins;

            pins = _pin_repo.getAllInArea(latStart, longStart, latRange, longRange);

            if (pins == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets all of the pins that a specific user has placed.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of NotFound() if no pins, or Ok(pins) if there are pins.
        ///</returns>
        [HttpGet("getPins")]
        public JsonResult getPins()
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            int uid = -1;

            uid = _user_repo.getId(user);

            List<Pin> pins = _pin_repo.getAllByUser(uid);

            if (pins == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Creates a pin.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of Ok(pin), where the pin is the one added to the database.
        ///</returns>
        [HttpPost("create")]
        public JsonResult create(Pin pin)
        {
            var identity = (User.Identity as ClaimsIdentity);

            User user = new User(identity);

            int uid = -1;

            uid = _user_repo.getId(user);

            pin.UserId= uid;

            _pin_repo.add(pin);

            return new JsonResult(Ok(pin));
        }

        /// <summary>
        /// Deletes a pin.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of Ok(pin), where the pin is the one added to the database.
        ///</returns>
        [HttpPost("delete/{pinId}")]
        public JsonResult delete(int pinId)
        {
            bool result = _pin_repo.delete(pinId);

            return new JsonResult(Ok(result));
        }

        /// <summary>
        /// Puts the supercharger pins into the database
        /// </summary>
        /// <returns>
        /// A JsonResult of Ok(0), and adds all of the supercharger pins to the database
        ///</returns>
        [HttpPost("initializeSuperchargers")]
        public JsonResult addSuperchargerPins()
        {
            AddingSuperchargerData.AddSuperchargers(_pin_repo);

            return new JsonResult(Ok(0));
        }

        /// <summary>
        /// Puts the gas and diesel pins into the database
        /// </summary>
        /// <returns>
        /// A JsonResult of Ok(0), and adds all of the gas and diesel pins to the database
        ///</returns>
        [HttpPost("initializeGas")]
        public JsonResult addGasPins()
        {
            AddingGasData.AddGas(_pin_repo);

            return new JsonResult(Ok(0));
        }

        /// <summary>
        /// Puts the alternative fuel pins (electric) into the database
        /// </summary>
        /// <returns>
        /// A JsonResult of Ok(0), and adds all of the alternative fuel pins to the database
        ///</returns>
        [HttpPost("initializeAltFuel")]
        public JsonResult addAltFuelPins()
        {
            AddingAltFuelData.AddAltFuel(_pin_repo);

            return new JsonResult(Ok(0));
        }

        /// Gets a pin based on name search term
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of NotFound() if it's not found or 
        ///Ok(pin) with the pin found.
        ///</returns>
        [HttpGet("getName/{searchTerm}")]
        public JsonResult getName(string searchTerm)
        {

            List<Pin> pins = _pin_repo.getByName(searchTerm);

            if (pins == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets all pins with a certain tag from the database
        /// </summary>
        /// <returns>
        /// A JsonResult of Ok(pins), where pins are the asked for pins
        ///</returns>
        [HttpPost("getAllPinsByTag")]
        public JsonResult getAllPinsByTag(List<int> tags, double latStart = 0, double longStart = 0, double latRange = 1, double longRange = 1)
        {
            List<Pin> pins;

            if(latStart == 0 && longStart == 0 && latRange == 0 && longRange == 0)
                pins = _pin_repo.getAllByTag(tags); //Defaults to oregon
            else
                pins = _pin_repo.getAllByTag(tags, latStart, longStart, latRange, longRange);

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets all pins with a similar address from the database
        /// </summary>
        /// <returns>
        /// A JsonResult of Ok(pins), where pins are the asked for pins
        ///</returns>
        [HttpPost("getAllPinsByAddress")]
        public JsonResult getAllPinsByAddress(string address)
        {
            List<Pin> pins = _pin_repo.getAllByAddress(address);

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets a pin based on city search term
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of NotFound() if it's not found or 
        ///Ok(pin) with the pin found.
        ///</returns>
        [HttpGet("getCity/{searchTerm}")]
        public JsonResult getCity(string searchTerm)
        {

            List<Pin> pins = _pin_repo.getByCity(searchTerm);

            if (pins == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets the average review result which is up_vote - down_vote
        /// </summary>
        /// <returns>
        /// Returns the review
        ///</returns>
        [HttpGet("getReview/{pinid}")]
        public JsonResult getReview(int pinid)
        {
            double review = _pin_repo.getAverageVote(pinid);

            return new JsonResult(Ok(review));
        }

        /// <summary>
        /// Gets a list of pins from a search term.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult with the pins.
        ///</returns>
        [HttpGet("getGlobal/{searchTerm}")]
        public JsonResult getGlobal(string searchTerm)
        {

            List<Pin> pins = _pin_repo.globalSearch(searchTerm);

            if (pins == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pins));
        }

        /// <summary>
        /// Gets all of the pins that a specific user has placed.
        /// </summary>
        /// <returns>
        /// Returns a JsonResult of NotFound() if no pins, or Ok(pins) if there are pins.
        ///</returns>
        [HttpGet("getUserStickers/{user}")] //TODO: Can't users change this? Verify user.
        public JsonResult getUserStickers(int uid, double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0)
        {
            List<Pin> pins;

            if(latStart == 0 && longStart == 0 && latRange == 0 && longRange == 0)
                pins = _pin_repo.getStickers(uid); //Defaults to oregon
            else
                pins = _pin_repo.getStickers(uid, latStart, longStart, latRange, longRange);

            if (pins == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(pins));
        }

        [HttpPost("autodelete/{pinId}")]
        public JsonResult autodelete(int pinId)
        {
            return new JsonResult(_pin_repo.autoRemove(pinId));
        }

    }
}
