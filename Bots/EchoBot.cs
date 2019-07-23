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
            skypeonlinehelper = new SkypeOnlineHelper();
            string upn = turnContext.Activity.Text;
            System.Diagnostics.Trace.WriteLine(upn);
            string myanswer = $"For user {upn}, here are the telephony details : ";
            System.Diagnostics.Trace.WriteLine(myanswer);
            skypeonlinehelper.getUserInfo(upn);
            Thread.Sleep(20000);
            myanswer += skypeonlinehelper.responseData;
            System.Diagnostics.Trace.WriteLine(myanswer);
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
