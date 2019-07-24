// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Connector;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        private string responseData { get; set; }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string upn = turnContext.Activity.Text;
            string currentUser = turnContext.Activity.From.Name;

            string myanswer = $"Hello {currentUser}, I am getting telephony details for user {upn}, please wait...";
            await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
            HttpClient client = new HttpClient();
            string apiUrl = "https://testneoswitv2v1.azurewebsites.net/api/HttpTriggerPowerShell1";
            client.BaseAddress = new Uri(apiUrl);
            var response = await client.GetAsync("?upn=" + upn);
            this.responseData = await response.Content.ReadAsStringAsync();
            myanswer = await response.Content.ReadAsStringAsync();
            await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                }
            }
        }
    }
}
