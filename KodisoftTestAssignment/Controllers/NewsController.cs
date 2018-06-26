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

        // GET news
        [HttpGet]
        public string Get()
        {
            // Return all Feed Collections for this user
            _logger.LogTrace("User " + _userManager.GetUserAsync(HttpContext.User).Id + " called method NewsController.Get()");

            var feedCollectionList = _newsServices.GetUserFeedCollections(new GetUserFeedCollectionsRequest
            {
                UserId = _userManager.GetUserId(User),

            });

            var json = JsonConvert.SerializeObject(feedCollectionList);

            return json;
        }

        // GET news/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            // Return FeedCollection with specified id
            _logger.LogTrace("User " + _userManager.GetUserAsync(HttpContext.User).Id + " called method NewsController.Get(int)");

            var feedCollection = _newsServices.GetFeedCollection(new GetFeedCollectionRequest
            {
                UserId = id
            });

            var json = JsonConvert.SerializeObject(feedCollection);

            return json;
        }

        // POST news
        [HttpPost]
        public int Post([FromBody]string title)
        {
            // Create Feed Collection with specified name
            _logger.LogTrace("User " + _userManager.GetUserAsync(HttpContext.User).Id + " called method NewsController.Post(string)");

            var id = _newsServices.CreateFeedCollection(new CreateFeedCollectionRequest
            {
                UserId = _userManager.GetUserId(User),
                Title = title
            });

            return id;

        }

        // PUT news/5
        [HttpPut]
        public void Put([FromBody]int collectionId, [FromBody]int feedId)
        {
            // Add feed to a collection
            _logger.LogTrace("User " + _userManager.GetUserAsync(HttpContext.User).Id + " called method NewsController.Put(int, int)");

            _newsServices.AddFeedToCollection(new AddFeedToCollectionRequest
            {
                UserId = _userManager.GetUserId(User),
                CollectionId = collectionId,
                FeedId = feedId
            });

        }
    }
}
