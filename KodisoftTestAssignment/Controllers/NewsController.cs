using KodisoftTestAssignment.Requests;
using KodisoftTestAssignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        // Return all Feed Collections for this user
        // GET news
        [HttpGet]
        public string Get()
        {
            
            var userId = _userManager.GetUserId(User);
            _logger.LogInformation("User " + userId + " called method NewsController.Get()");

            var feedCollectionList = _newsServices.GetUserFeedCollections(new GetUserFeedCollectionsRequest
            {
                UserId = userId,

            });

            var json = JsonConvert.SerializeObject(feedCollectionList);

            return json;
        }

        // Return FeedCollection with specified id
        // GET news/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var userId = _userManager.GetUserId(User);
            _logger.LogInformation("User " + userId + " called method NewsController.Get(int)");

            var feedCollection = _newsServices.GetFeedCollection(new GetFeedCollectionRequest
            {
                UserId = userId,
                FeedCollectionID = id
            });

            var json = JsonConvert.SerializeObject(feedCollection);
            return json;
        }

        // Create Feed Collection with specified name
        // POST news
        [HttpPost]
        public int Post([FromBody]CreateFeedCollectionRequest request)
        {
            request.UserId = _userManager.GetUserId(User);
            _logger.LogInformation("User " + request.UserId + " called method NewsController.Post(string)");

            var id = _newsServices.CreateFeedCollection(request);
            return id;

        }

        // Add new feed
        // POST news/addfeed
        [HttpPost]
        [Route("addfeed")]
        public void AddFeed([FromBody]string url)
        {
            var userId = _userManager.GetUserId(User);
            _logger.LogInformation("User " + userId + " called method NewsController.AddFeed(string)");
            _newsServices.AddFeed(url);
        }

        // Add feed to a collection
        // PUT news
        [HttpPut]
        public void Put([FromBody]AddFeedToCollectionRequest request)
        {
            request.UserId = _userManager.GetUserId(User);
            _logger.LogInformation("User " + request.UserId + " called method NewsController.Put(int, int)");
            
            _newsServices.AddFeedToCollection(request);

        }
    }
}
