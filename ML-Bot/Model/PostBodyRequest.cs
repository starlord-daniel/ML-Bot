using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples.Model
{
    public class PostBodyRequest
    {
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }   
}
