using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using PresentationApplication.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace PresentationApplication.Controllers
{
    public class HomeController : Controller
    {
        public string AuthToken;
        private readonly IRestClient _restClient;
        private readonly CookieContainer _cookieJar;

        public HomeController(IRestClient restClient)
        {
            _restClient = restClient;

            _cookieJar = new CookieContainer();
            _restClient.CookieContainer = _cookieJar;

            _restClient.BaseUrl = new System.Uri("http://localhost:50312/api/account/sign-in");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\n\t\"email\": \"someuser@somewhere.com\",\n\t\"password\": \"123456#User\"\n}", ParameterType.RequestBody);
            var response = _restClient.Execute(request);

            var sessionCookie = response.Cookies.SingleOrDefault(x => x.Name == ".AspNetCore.Identity.Application");
            if (sessionCookie != null)
            {
                _cookieJar.Add(new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain));
            }

            var token = JsonConvert.DeserializeObject<Token>(response.Content);
            AuthToken = token.Access_Token;
        }

        public IActionResult Index()
        {
            _restClient.BaseUrl = new System.Uri("http://localhost:50312/api/news");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + AuthToken);
            var response = _restClient.Execute(request);
            var json = response.Content;
            json = Regex.Unescape(json);
            json = json.Substring(1, json.Length - 2);
            var feedCollections = JsonConvert.DeserializeObject<List<FeedCollection>>(json);

            ViewData["feedCollections"] = feedCollections;

            return View();
        }

        public IActionResult FeedCollection(int id)
        {
            _restClient.BaseUrl = new System.Uri("http://localhost:50312/api/news/" + id);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + AuthToken);
            var response = _restClient.Execute(request);
            var json = response.Content;
            json = Regex.Unescape(json);
            json = json.Substring(1, json.Length - 2);
            var feedCollection = JsonConvert.DeserializeObject<FeedCollection>(json);
            var feedsInFC = feedCollection.FeedCollectionFeeds.Select(fcf => fcf.Feed).ToList();

            _restClient.BaseUrl = new System.Uri("http://localhost:50312/api/news/feeds");
            request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + AuthToken);
            response = _restClient.Execute(request);
            json = response.Content;
            json = Regex.Unescape(json).Substring(1, json.Length - 2);
            var feeds = JsonConvert.DeserializeObject<List<Feed>>(response.Content.Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}").Replace("\"[", "[").Replace("]\"", "]"));
            
            feeds = feeds.Where(f => !feedsInFC.Select(f1 => f1.ID).Contains(f.ID)).ToList();
            
            var news = feedsInFC.SelectMany(f => f.Items).OrderByDescending(i => i.PublishDate).ToList();

            ViewData["news"] = news;
            ViewData["feedCollection"] = feedCollection;
            ViewData["feedsInFC"] = feedsInFC;
            ViewData["notUserFeeds"] = feeds;
            ViewData["token"] = AuthToken;

            return View();
        }

        [HttpPost]
        public IActionResult AddFeedToCollection([FromBody] AddFeedToCollectionRequest request)
        {
            _restClient.BaseUrl = new System.Uri("http://localhost:50312/api/news");
            var restRequst = new RestRequest(Method.PUT);
            restRequst.AddHeader("Cache-Control", "no-cache");
            restRequst.AddHeader("Content-Type", "application/json");
            restRequst.AddHeader("Authorization", "Bearer " + AuthToken);
            restRequst.AddParameter("undefined", "{\n\t\"feedCollectionId\": " + request.FeedCollectionId + ",\n\t\"feedId\": " + request.FeedId + "\n}", ParameterType.RequestBody);
            IRestResponse response = _restClient.Execute(restRequst);

            return Ok();
        }
    }
}
