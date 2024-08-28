using Newtonsoft.Json;

namespace twitterAPI.Models
{
    public class PostTweetRequestDto
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}