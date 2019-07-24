// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Helpers;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {

        private SkypeOnlineHelper skypeonlinehelper;
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(turnContext.Activity.ServiceUrl));
            skypeonlinehelper = new SkypeOnlineHelper();
            string upn = turnContext.Activity.Text;
            string currentUser = turnContext.Activity.From.Name;

            string myanswer = $"Hello {currentUser}, I am getting telephony details for user {upn}, please wait...";
            Activity reply = turnContext.Activity.CreateReply(myanswer);
            var msgToUpdate = await connector.Conversations.ReplyToActivityAsync(reply);
            await skypeonlinehelper.getUserInfo(upn);
            //Thread.Sleep(20000);
            string updatedReply = skypeonlinehelper.responseData.Trim();
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
