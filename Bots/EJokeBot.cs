// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EJokeBot.Bots
{
    public class EJokeBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var builder = new KernelBuilder();
            builder.WithAzureChatCompletionService(
                     configuration["AOAISettings:DeploymentName"], // Azure OpenAI Deployment Name
                     configuration["AOAISettings:Endpoint"],       // Azure OpenAI Endpoint
                     configuration["AOAISettings:ApiKey"]);        // Azure OpenAI Key
            var kernel = builder.Build();
            var prompt = @"貴方は漫才師です。以下の文章・単語を使ってエンジニアが楽しめるジョークを１００文字位で作ってください。

{{$input}}";
            var joke = kernel.CreateSemanticFunction(prompt);

            // output diagnostic message
            System.Diagnostics.Trace.TraceError($"user input is {turnContext.Activity.Text}");

            var reply = await joke.InvokeAsync(turnContext.Activity.Text, kernel); //$"Echo v2: {turnContext.Activity.Text}";
            var replyText = reply.ToString();

            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Azure OpenAI がエンジニアジョークを作るので、キーワードや短い文章を入れてね！";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
