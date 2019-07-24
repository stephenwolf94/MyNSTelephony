// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

        //private SkypeOnlineHelper skypeonlinehelper;
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(turnContext.Activity.ServiceUrl));
            //skypeonlinehelper = new SkypeOnlineHelper();
            string upn = turnContext.Activity.Text;
            string currentUser = turnContext.Activity.From.Name;

            string myanswer = $"Hello {currentUser}, I am getting telephony details for user {upn}, please wait...";
            Activity reply = turnContext.Activity.CreateReply(myanswer);
            var msgToUpdate = await connector.Conversations.ReplyToActivityAsync(reply);

            //await skypeonlinehelper.getUserInfo(upn);
            HttpClient client = new HttpClient();
            string apiUrl = "https://testneoswitv2v1.azurewebsites.net/api/HttpTriggerPowerShell1";
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetAsync("?upn=" + upn);
            //this.responseData = await response.Content.ReadAsStringAsync();
            string updatedReply = await response.Content.ReadAsStringAsync();
            //await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
            connector.Conversations.UpdateActivityAsync(reply.Conversation.Id, msgToUpdate.Id, updatedReply);
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
