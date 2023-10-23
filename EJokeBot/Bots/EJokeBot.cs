// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EJokeBot.Services;

namespace EJokeBot.Bots;

public class EJokeBot : ActivityHandler
{
    private readonly IJokeGenerator _jokeGenerator;
    private readonly ILogger<EJokeBot> _logger;

    public EJokeBot(IJokeGenerator jokeGenerator, ILogger<EJokeBot> logger)
    {
        _jokeGenerator = jokeGenerator;
        _logger = logger;
    }

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        // output diagnostic message
        _logger.LogTrace("user input is {userInput}", turnContext.Activity.Text);
        var replyText = await _jokeGenerator.GenerateJokeAsync(turnContext.Activity.Text, cancellationToken);
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
