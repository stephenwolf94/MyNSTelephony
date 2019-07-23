// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        private SkypeOnlineHelper skypeonlinehelper;
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string upn = turnContext.Activity.Text;
            string myanswer = $"For user{upn}, here are the telephony details : ";
            await skypeonlinehelper.getUserInfo(upn);
            myanswer += skypeonlinehelper.responseData;
            await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    skypeonlinehelper = new SkypeOnlineHelper();
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                }
            }
        }
    }
}
