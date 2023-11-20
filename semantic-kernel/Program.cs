using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.Core;

var basePath = Directory.GetCurrentDirectory();
var configurationFilePath = Path.Combine(basePath, "appsettings.json");
var configurationBuilder = new ConfigurationBuilder().AddJsonFile(configurationFilePath);
var configuration = configurationBuilder.Build();

var tenantId = configuration["tenantId"];
var clientId = configuration["clientId"];
var clientSecret = configuration["clientSecret"];
var scope ="api://"+clientId+"/.default";

var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
var accessToken = credential.GetToken(new TokenRequestContext(new[] { scope }));

var builder = new KernelBuilder();

builder.WithAzureChatCompletionService(
    configuration["deploymentName"] ?? throw new ArgumentNullException(),
    configuration["openAiEndpoint"] ?? throw new ArgumentNullException(),
    new AccessTokenCredential(accessToken)       
        );            

var kernel = builder.Build();

var prompt = @"What is the city where the James Bond movie ""No Time to Die"" starts ?";

var question = kernel.CreateSemanticFunction(prompt, maxTokens: 100);

Console.WriteLine(await question.InvokeAsync(prompt));