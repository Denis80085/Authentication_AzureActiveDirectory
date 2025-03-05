using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace WeatherClient
{
    public class AuthenticationExecuter
    {
        private readonly AuthConfig _config;

        public AuthenticationExecuter(AuthConfig config)
        {
            _config = config;
        }

        public async Task<string?> GetTokenAsync()
        {
            var app = ConfidentialClientApplicationBuilder.Create(_config.ClientId)
                        .WithClientSecret(_config.ClientSecret)
                        .WithAuthority(new Uri(_config.Authority))
                        .WithTenantId(_config.TenantId)
                        .Build();
            
            string[] ResourceIds =  { _config.ResourceId };

            AuthenticationResult? result = null;

            try
            {
                result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Token  Aquaired \n");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();

                return result.AccessToken;
            }
            catch(MsalClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Token could not be aquaired\n");
                Console.WriteLine(ex.ToString());
                Console.ResetColor();

                return null;
            }
        }

        public async Task SendGetRequestAsync(string Token)
        {
            using HttpClient client = new();

            var defRequestHeaders = client.DefaultRequestHeaders;

            if(defRequestHeaders.Accept == null || !defRequestHeaders.Accept
            .Any(m => m.MediaType is "application/json"))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(
                    "application/json"
                ));
            }

            defRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await client.GetAsync(_config.BaseAddress);

            if(response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                string json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Authefication succeded\n");
                Console.WriteLine(json);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Authefication failed\n");
                string res = response.ToString();
                Console.WriteLine(res);
                Console.ResetColor();
            }
            Console.ResetColor();
        }
    }
}