using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Bot.Samples.Api
{
    public static class Translator
    {
        public async static Task<string> TranslateText(string text, string key, string baseUrl, string toLanguage = "de", string fromLanguage = "en")
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            string uri = $"{baseUrl}?to={toLanguage}&from={fromLanguage}&text={System.Net.WebUtility.UrlEncode(text)}&category=generalnn";

            HttpResponseMessage response = await client.GetAsync(uri);

            string result = await response.Content.ReadAsStringAsync();

            var content = XElement.Parse(result).Value;
            return content;
        }

        public async static Task<Activity> TranslateActivity(Activity activity, string key, string baseUrl, string toLanguage = "en", string fromLanguage = "de")
        {
            var newActivity = activity;
            newActivity.Text = await TranslateText(activity.Text, key, baseUrl, toLanguage, fromLanguage);
            newActivity.Speak = await TranslateText(activity.Speak, key, baseUrl, toLanguage, fromLanguage);

            if (newActivity.Attachments != null)
            {
                foreach (var item in newActivity.Attachments)
                {
                    if (item.ContentType == "application/vnd.microsoft.card.hero")
                    {
                        var heroCard = (HeroCard)item.Content;
                        if (string.IsNullOrEmpty(heroCard.Title)) heroCard.Title = await TranslateText(heroCard.Title, key, baseUrl, toLanguage, fromLanguage);
                        if (string.IsNullOrEmpty(heroCard.Subtitle)) heroCard.Title = await TranslateText(heroCard.Title, key, baseUrl, toLanguage, fromLanguage);
                        if (string.IsNullOrEmpty(heroCard.Text)) heroCard.Title = await TranslateText(heroCard.Title, key, baseUrl, toLanguage, fromLanguage);

                        foreach (var button in heroCard.Buttons)
                        {
                            if (string.IsNullOrEmpty(button.DisplayText)) heroCard.Title = await TranslateText(button.DisplayText, key, baseUrl, toLanguage, fromLanguage);
                            if (string.IsNullOrEmpty(button.Text)) heroCard.Title = await TranslateText(button.Text, key, baseUrl, toLanguage, fromLanguage);
                            if (string.IsNullOrEmpty(button.Title)) heroCard.Title = await TranslateText(button.Title, key, baseUrl, toLanguage, fromLanguage);
                        }
                    }
                    // Expand here
                }
            }

            return newActivity;
        }

        public async static Task<string> DetectLanguage(string text, string key, string baseUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            string uri = baseUrl + "?text=" + System.Net.WebUtility.UrlEncode(text);

            HttpResponseMessage response = await client.GetAsync(uri);

            string result = await response.Content.ReadAsStringAsync();

            var content = XElement.Parse(result).Value;
            return content;
        }
    }
}
