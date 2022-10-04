using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace StockBox.Configuration.App
{
    public class StockboxConfiguration : IStockBoxConfiguration
    {
        private readonly IConfiguration _configuration;

        private static StockboxConfiguration _instance;

        private StockboxConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EEnvironment Environment { get { return MapEnvironment(); } }


        public static StockboxConfiguration GetInstance()
        {
            if (_instance == null)
            {
                var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                _instance = new StockboxConfiguration(configuration);
            }

            return _instance;
        }


        private EEnvironment MapEnvironment()
        {
            string env = _configuration["Environment"];
            return env switch
            {
                "debug" => EEnvironment.eDebug,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
