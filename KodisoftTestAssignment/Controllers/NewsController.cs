using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KodisoftTestAssignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace KodisoftTestAssignment.Controllers
{
    [Route("news")]
    public class NewsController : Controller
    {
        private const string apikey = "f66f7f3881f8473aa596e6898bfe1f24";

        // GET news
        [HttpGet]
        public string Get()
        {
            // Recieving top-trending headlines from API

            var url = "https://newsapi.org/v2/top-headlines?" + "country=us&" + "apiKey=" + apikey;

            var json = new WebClient().DownloadString(url);
            

            // var newsJson = JsonConvert.SerializeObject(news);

            return json;
        }
        
        // GET news/feedlist
        [HttpGet("/feedlist")]
        public List<Feed> GetFeedList()
        {
            var feedList = new List<Feed>();

            // Here we should have call to service method, that will get all feeds of this user from DB

            return feedList;
        }

        // GET news/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var url = "https://newsapi.org/v2/top-headlines?" + "country=us&" + "apiKey=" + apikey;

            var json = new WebClient().DownloadString(url);

            System.Diagnostics.Debug.WriteLine(json);

            return json;
        }

        // POST news
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT news/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE news/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
