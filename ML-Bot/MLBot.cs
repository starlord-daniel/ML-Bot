using Microsoft.Bot.Builder;
using Microsoft.Bot.Samples.Api;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Bot.Samples
{
    public class MLBot : IBot
    {
        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type is ActivityTypes.Message)
            {
                
            }
        }

        
    }
}