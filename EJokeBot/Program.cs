// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using EJokeBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EJokeBot.Options;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Options;
using EJokeBot.Services;

var builder = WebApplication.CreateBuilder(args);

// https://learn.microsoft.com/ja-jp/dotnet/core/extensions/options
builder.Services.AddOptions<AOAISettings>()
    .BindConfiguration(nameof(AOAISettings))
    .ValidateDataAnnotations();

// https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/KernelSyntaxExamples/Example40_DIContainer.cs
builder.Services.AddTransient(sp =>
{
    var aoaiSettings = sp.GetRequiredService<IOptions<AOAISettings>>().Value;
    return Kernel.Builder
        .WithAzureChatCompletionService(
            aoaiSettings.DeploymentName,
            aoaiSettings.Endpoint,
            aoaiSettings.ApiKey)
        .Build();
});

builder.Services.AddHttpClient().AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.MaxDepth = HttpHelper.BotMessageSerializerSettings.MaxDepth;
});

// Create the Bot Framework Authentication to be used with the Bot Adapter.
builder.Services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

// Create the Bot Adapter with error handling enabled.
builder.Services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

// Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
builder.Services.AddTransient<IBot, EJokeBot.Bots.EJokeBot>();

// Application services
builder.Services.AddTransient<IJokeGenerator, JokeGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseWebSockets();

app.UseAuthorization();

app.MapControllers();

app.Run();
