/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 12/28/2022
*
* Purpose: Contains PinTag Controllers. Probably temporary, since should be handled in PinController.cs.

*
************************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TravelCompanionAPI.Models;
using TravelCompanionAPI.Data;

namespace TravelCompanionAPI.Controllers
{
        /// <summary>
        /// Default route controller.
        /// </summary>
        /// <returns>
        /// S
        ///</returns>
    [Route("pintags")]
    [ApiController]
    [Authorize]
    public class PinTagController : ControllerBase
    {
        private IPinTagRepository _repo;
        public PinTagController(IPinTagRepository repo)
        {
            _repo = repo;
        }

        //[HttpGet("getTag/{tag_id}")]
        //public JsonResult get(int tid)
        //{

        //    PinTag pintag = _repo.getByTagId(tid);

        //    if (pintag == null)
        //    {
        //        return new JsonResult(NotFound());
        //    }

        //    return new JsonResult(Ok(pintag));
        //}

        [HttpGet("all")]
        public JsonResult getAll()
        {
            List<PinTag> pintags = _repo.getAll();

            return new JsonResult(Ok(pintags));
        }

        [HttpPost("create")]
        public JsonResult create(PinTag pintag)
        {
            _repo.add(pintag);

            return new JsonResult(Ok(pintag));
        }
    }
}
