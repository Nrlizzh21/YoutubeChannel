using Microsoft.AspNetCore.Mvc;
using YoutubeChannel.Services;
using YoutubeChannel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoutubeChannel.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        // Display Search Page 
        public IActionResult Index()
        {
            ViewBag.IsSubmitted = false; // Initialize to false
            return View(new List<YoutubeChannelModel>()); // Pass an empty list initially
        }

        // Handle the search query 
        [HttpPost]
        public async Task<IActionResult> Search(string query, string pageToken)
        {
            // Ensure that the query is not null or empty before proceeding
            if (string.IsNullOrWhiteSpace(query))
            {
                ViewBag.Query = query;
                ViewBag.IsSubmitted = true; // Set to true since the form was submitted
                ViewBag.NoResultsMessage = "Please enter a search term."; // Message for empty query
                return View("Index", new List<YoutubeChannelModel>()); // Return an empty list if no query is given
            }

            var videos = await _youtubeService.SearchChannelAsync(query, pageToken);

            // Set the ViewBag properties for pagination
            ViewBag.Query = query;
            ViewBag.IsSubmitted = true; // Indicate that the form was submitted
            ViewBag.NextPageToken = videos.NextPageToken; 
            ViewBag.PreviousPageToken = videos.PreviousPageToken; 

            if (videos.Items == null || !videos.Items.Any())
            {
                ViewBag.NoResultsMessage = "No channels found. Please try a different search query."; // Message for no results
            }
            else
            {
                ViewBag.NoResultsMessage = null; // Clear the message if channels are found
            }

            return View("Index", videos.Items);
        }

        public async Task<IActionResult> Details(string id)
        {
            var channel = await _youtubeService.GetChannelDetailsAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }
    }
}
