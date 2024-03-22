/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 1/21/2023
*
* Purpose: Contains Cellular Controllers.
*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TravelCompanionAPI.Models;
using TravelCompanionAPI.Data;

namespace TravelCompanionAPI.Controllers
{
    /// <summary>
    /// Default cellular controller.
    /// </summary>
    [Route("h3_oregon_data")]
    [ApiController]
    public class CellularController : ControllerBase
    {
        //The repository obtained through dependency injection.
        private ICellularRepository _repo;

        public CellularController(ICellularRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("saveData/{state}")]
        public JsonResult saveData(string state)
        {

            _repo.SaveToDatabase(state);

            return new JsonResult(Ok());
        }

        [HttpGet("allH3/{state}")]
        public JsonResult getAllH3(string state)
        {
            List<string> h3List = _repo.getAllH3(state);

            return new JsonResult(Ok(h3List));
        }

        [HttpGet("allCoordsSingle/{max_pass}/{latMin}/{lngMin}/{latMax}/{lngMax}")]
        public JsonResult getAllCoordsSingle(int max_pass, float latMin, float lngMin, float latMax, float lngMax)
        {
            List<float> latLngList = _repo.getAllCoordsThreaded(max_pass, latMin, lngMin, latMax, lngMax);

            return new JsonResult(Ok(latLngList));
        }

        [HttpGet("allCoordsSingleThreadless/{max_pass}/{pass}/{latMin}/{lngMin}/{latMax}/{lngMax}")]
        public JsonResult getAllCoordsSingleThreadless(int max_pass, int pass, float latMin, float lngMin, float latMax, float lngMax)
        {
            List<float> latLngList = _repo.getAllCoordsSingle(max_pass, pass, latMin, lngMin, latMax, lngMax);

            return new JsonResult(Ok(latLngList));
        }
    }
}
