using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KodisoftTestAssignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace KodisoftTestAssignment.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        // GET news
        [HttpGet]
        public string Get()
        {
            // Recieving top-trending headlines from API
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = "Bearer f66f7f3881f8473aa596e6898bfe1f24";

            var url = "https://newsapi.org/v2/top-headlines?" + "country=us&" + "apiKey=f66f7f3881f8473aa596e6898bfe1f24";

            var json = webClient.DownloadString(url);
            

            // var newsJson = JsonConvert.SerializeObject(news);

            return json;
        }

        // GET news/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = "Bearer f66f7f3881f8473aa596e6898bfe1f24";
            var url = "https://newsapi.org/v2/top-headlines?" + "country=us&";

            var json = webClient.DownloadString(url);

            System.Diagnostics.Debug.WriteLine(json);

            return json;
        }

        // GET news/feedlist
        [Route("/feedlist")]
        [HttpGet]
        public List<FeedCollection> GetFeedList()
        {
            var feedList = new List<FeedCollection>();

            // Here we should have call to service method, that will get all feeds of this user from DB

            return feedList;
        }

        // POST news
        [HttpPost]
        public int Post([FromBody]string name)
        {
            // Create collection with specified name
            // Collection id should be returned
            var id = 0; // placeholder

            return id;

        }

        // PUT news/5
        [HttpPut("{collectionId}")]
        public void Put(int collectionId, [FromBody]string feedId)
        {
            // Add feed to a collection
        }

        // DELETE news/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
