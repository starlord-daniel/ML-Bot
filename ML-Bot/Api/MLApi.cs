using Microsoft.Bot.Samples.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples.Api
{
    public static class MLApi
    {
        private const string SERVICE_URL = "<ENDPOINT>";

        public static async Task<string> GetPredictionAsync(string imageUrl)
        {
            try
            {
                HttpClient client = new HttpClient();

                PostBodyRequest requestObject = new PostBodyRequest
                {
                    ImageUrl = imageUrl
                };

                var postString = JsonConvert.SerializeObject(requestObject);

                HttpContent postContent = new StringContent(postString, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(SERVICE_URL, postContent);

                var responseString = await response.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeObject<PostBodyResponse>(responseString);

                return responseObject.description;
            }
            catch (Exception)
            {
                return "This is not an url I can work with, I'm so sorry!";
            }
            
        }
    }
}
