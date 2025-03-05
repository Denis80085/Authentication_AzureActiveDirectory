using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using WeatherClient;

AuthConfig config = AuthConfig.ReadJsonFromSecrets();
var AuhtExec = new AuthenticationExecuter(config);

Console.WriteLine($"Authority: {config.Authority}");

var app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
            .WithClientSecret(config.ClientSecret)
            .WithAuthority(new Uri(config.Authority))
            .Build();

string[] ResourceIds = {
    config.ResourceId
};

Console.WriteLine("Getting Token....");

string? token = await AuhtExec.GetTokenAsync();

if(token != null)
{
    await AuhtExec.SendGetRequestAsync(token);
}