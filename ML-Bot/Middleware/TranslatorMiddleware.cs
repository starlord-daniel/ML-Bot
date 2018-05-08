using Microsoft.Bot.Builder;
using Microsoft.Bot.Samples.Api;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples.Middleware
{
    public class TranslatorMiddleware : IMiddleware
    {
        // Shortcut to hide marked code: Ctrl + M , Ctrl + H
        const string TEXT_TRANSLATION_API_SUBSCRIPTION_KEY = "<YOUR KEY HERE>";
        const string TEXT_TRANSLATION_API_ENDPOINT = "https://api.microsofttranslator.com/V2/Http.svc/Translate";
        const string LANGUAGE_DETECTION_ENDPOINT = "https://api.microsofttranslator.com/V2/Http.svc/Detect";

        public async Task OnTurn(ITurnContext context, MiddlewareSet.NextDelegate next)
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                var language = await Translator.DetectLanguage(context.Activity.Text,
                    TEXT_TRANSLATION_API_SUBSCRIPTION_KEY,
                    LANGUAGE_DETECTION_ENDPOINT);

                await next();

                string testUrl = GetImageUrl(context.Activity.Text);

                string mlResponse = await MLApi.GetPredictionAsync(testUrl);

                var mlResponseTranslation = await Translator.TranslateText(mlResponse,
                    TEXT_TRANSLATION_API_SUBSCRIPTION_KEY,
                    TEXT_TRANSLATION_API_ENDPOINT,
                    toLanguage: language);

                var mlActivity = CreateMLResponseActivity(context, testUrl, mlResponseTranslation);

                // This simple middleware reports the request type and if we responded
                await context.SendActivity(mlActivity);
            }
        }

        private string GetImageUrl(string input)
        {
            string imageUrl = "";

            string urlPattern = @"([h][t][t][p][a-z|A-Z|0-9|:\/.?=_&-]+)";
            Regex urlRegex = new Regex(urlPattern);

            MatchCollection matches = urlRegex.Matches(input);

            if (matches.Count > 0)
            {
                imageUrl = matches.FirstOrDefault().Value;
            }

            return imageUrl;
        }

        private IMessageActivity CreateMLResponseActivity(ITurnContext context, string testUrl, string mlResponse)
        {
            var photoActivity = context.Activity.AsMessageActivity();
            photoActivity.Attachments = new List<Attachment>()
            {
                new HeroCard
                {
                    Title = "ML Answer",
                    Text = mlResponse,
                    Images = new List<CardImage>
                    {
                        new CardImage
                        {
                            Url = testUrl
                        }
                    }
                }.ToAttachment()
            };

            photoActivity.Text = "";

            return photoActivity;
        }


    }
}
