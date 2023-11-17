using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configureDelegate: config =>
    {
        config.AddJsonFile("./appsettings.json", optional: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient();
        services.AddHostedService<KafkaConsumerService>();
        services.Configure<ConsumerConfig>(hostContext.Configuration.GetSection("Kafka"));
        services.Configure<ApiSettings>(hostContext.Configuration.GetSection("ApiSettings"));
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();
