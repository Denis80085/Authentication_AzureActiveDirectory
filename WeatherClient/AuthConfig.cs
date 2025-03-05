using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherClient
{
    public class AuthConfig
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string Authority 
        { 
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, TenantId);
            } 
        }
        public string ClientSecret { get; set; }
        public string BaseAddress { get; set; }
        public string ResourceId { get; set; }

        public static AuthConfig ReadJsonFromSecrets()
        {
            IConfiguration config;

            var builder = new ConfigurationBuilder().AddUserSecrets<AuthConfig>();

            config = builder.Build();

            return config.GetSection("AuthConfig").Get<AuthConfig>();
        }
    }
}