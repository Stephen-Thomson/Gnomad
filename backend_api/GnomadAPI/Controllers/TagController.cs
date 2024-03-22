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

namespace TravelCompanionAPI.Controllers
{
    /// <summary>
    /// Default route controller.
    /// </summary>
    [Route("tags")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        //The repository obtained through dependency injection.
        private ITagRepository _repo;

        public TagController(ITagRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("get/{id}")]
        public JsonResult get(int id)
        {

            Tag tag = _repo.getById(id);

            if (tag == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(tag));
        }

        [HttpGet("all")]
        public JsonResult getAll()
        {
            List<Tag> tags = _repo.getAll();

            return new JsonResult(Ok(tags));
        }

        [HttpPost("create")]
        public JsonResult create(Tag tag)
        {
            _repo.add(tag);

            return new JsonResult(Ok(tag));
        }
    }
}
