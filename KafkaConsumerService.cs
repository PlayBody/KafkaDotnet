using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly HttpClient _httpClient;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly string _endpointUrl;

    public KafkaConsumerService(IOptions<ConsumerConfig> config, IOptions<ApiSettings> apiSettings, HttpClient httpClient, ILogger<KafkaConsumerService> logger)
    {
        _consumer = new ConsumerBuilder<Null, string>(config.Value).Build();
        _httpClient = httpClient;
        _logger = logger;
        _endpointUrl = apiSettings.Value.EndpointUrl;

        _logger.LogInformation("Service Constructor");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("myAwesomeTopic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = new { message = consumeResult.Message.Value };
                var jsonMessage = JsonConvert.SerializeObject(message);
                var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_endpointUrl, content, stoppingToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to forward data to {_endpointUrl}. Status code: {response.StatusCode}");
                } else
                {
                    _logger.LogInformation($"Response: Code={response.StatusCode} Message={response.Content.ReadAsStringAsync().Result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while consuming/forwarding data.");
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Unsubscribe();
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}