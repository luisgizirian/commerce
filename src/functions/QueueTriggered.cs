using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.Dapr;
using Microsoft.Azure.Functions.Worker;

namespace functions
{
    public class DaprQueueTriggered
    {
        private readonly ILogger _logger;

        public DaprQueueTriggered(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DaprQueueTriggered>();
        }

        [Function("DaprQueueTriggered")]
        public void Run([DaprTopicTrigger("pubsub", Topic = "orders")] string data)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {data}");
        }
    }
}
