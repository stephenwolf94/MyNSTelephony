// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
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
            string command = turnContext.Activity.Text;
            string currentUser = turnContext.Activity.From.Name;

            string commandsettings = Regex.Split(command, @"\s+").Where(s => s != string.Empty);

            if (!IsNullOrEmpty(commandsettings[0]))
            {
                switch (commandsettings[0])
                {
                    case "GetUserInfo":
                        string upn = commandsettings[1];
                        string userID = turnContext.Activity.From.AadObjectId;
                        string myanswer = $"Hello {currentUser}, I am getting telephony details for user {upn}, please wait...";
                        await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
                        GraphAPIs graphAPIs = new GraphAPIs();
                        string token = graphAPIs.GetToken();
                        graphAPIs.SendRequestUser(userID);
                        string groups = graphAPIs.GetResponseData();
                        myanswer = $"You are member of {groups}";
                        await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
                        HttpClient client = new HttpClient();
                        string apiUrl = "https://testneoswitv2v1.azurewebsites.net/api/HttpTriggerPowerShell1";
                        client.BaseAddress = new Uri(apiUrl);
                        var response = await client.GetAsync("?upn=" + upn);
                        this.responseData = await response.Content.ReadAsStringAsync();
                        myanswer = await response.Content.ReadAsStringAsync();
                        await turnContext.SendActivityAsync(MessageFactory.Text(myanswer), cancellationToken);
                        break;
                    default:
                        await turnContext.SendActivityAsync(MessageFactory.Text($"This command '{commandsettings[0]}' is not recognized"), cancellationToken);
                        break;
                }
            }
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
