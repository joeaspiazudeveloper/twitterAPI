using System.Text;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Models;
using twitterAPI.Models;

namespace twitterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public TweetsController(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        [HttpPost]
        public async Task<IActionResult> PostTweet(PostTweetRequestDto newTweet)
        {

            // var client = new TwitterClient("XX4E5tIQR3Vmr56Ct8mTS3Lbu",
            //     "aYOotVLyZAog8WZUpusWnVzBDys1pPSQcw3TuyxDvwcQceAw4G",
            //     "564602877-nHuA6KIFbC7nDOtCvVJ4eCl7AdBViJK5ynWESdfX",
            //     "joyfPEPCJdy8Q1ql8kJvP3jEHskxKeucc03RyNXLlHDSa");

            var apiKey = _configuration["Twitter:ApiKey"];
            var apiSecretKey = _configuration["Twitter:ApiSecretKey"];
            var accessToken = _configuration["Twitter:AccessToken"];
            var accessTokenSecret = _configuration["Twitter:AccessTokenSecret"];

             var client = new TwitterClient(apiKey, apiSecretKey, accessToken, accessTokenSecret);

            var result = await client.Execute.AdvanceRequestAsync(
                BuildTwitterRequest(newTweet, client));
            
            return Ok(result.Content);
        }

        private static Action<ITwitterRequest> BuildTwitterRequest(
            PostTweetRequestDto newTweet,
            TwitterClient client)
        {
            return (ITwitterRequest request) => 
            {
                var jsonBody = client.Json.Serialize(newTweet);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                request.Query.Url = "https://api.twitter.com/2/tweets";
                request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                request.Query.HttpContent = content;
            };
        }
    }
}