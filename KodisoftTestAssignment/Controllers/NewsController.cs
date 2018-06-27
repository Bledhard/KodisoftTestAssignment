using KodisoftTestAssignment.Requests;
using KodisoftTestAssignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KodisoftTestAssignment.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ILogger<NewsController> _logger;
        NewsServices _newsServices;

        public NewsController(
            ILogger<NewsController> logger,
            NewsServices newsServices,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _newsServices = newsServices;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all Feed Collections for this user
        /// </summary>
        /// <returns>Feed Collection in JSON format</returns>
        // GET news
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                _logger.LogInformation("User " + userId + " called method NewsController.Get()");

                var feedCollectionList = _newsServices.GetUserFeedCollections(new GetUserFeedCollectionsRequest
                {
                    UserId = userId,

                });

                var json = JsonConvert.SerializeObject(feedCollectionList);

                return Ok(json);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Get FeedCollection with specified id
        /// </summary>
        // GET news/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                _logger.LogInformation("User " + userId + " called method NewsController.Get(int)");

                var feedCollection = _newsServices.GetFeedCollection(new GetFeedCollectionRequest
                {
                    UserId = userId,
                    FeedCollectionID = id
                });

                var json = JsonConvert.SerializeObject(feedCollection);
                return Ok(json);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Create Feed Collection with specified name
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Feed Collection ID</returns>
        // POST news
        [HttpPost]
        public IActionResult Post([FromBody]CreateFeedCollectionRequest request)
        {
            try
            {
                request.UserId = _userManager.GetUserId(User);
                _logger.LogInformation("User " + request.UserId + " called method NewsController.Post(string)");

                var id = _newsServices.CreateFeedCollection(request);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Add new feed
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Feed ID</returns>
        // POST news/addfeed
        [HttpPost]
        [Route("addfeed")]
        public IActionResult AddFeed([FromBody]string url)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                _logger.LogInformation("User " + userId + " called method NewsController.AddFeed(string)");
                var id = _newsServices.AddFeed(url);
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Add feed to a collection
        /// </summary>
        /// <param name="request"></param>
        // PUT news
        [HttpPut]
        public IActionResult Put([FromBody]AddFeedToCollectionRequest request)
        {
            try
            {
                request.UserId = _userManager.GetUserId(User);
                _logger.LogInformation("User " + request.UserId + " called method NewsController.Put(int, int)");

                _newsServices.AddFeedToCollection(request);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
