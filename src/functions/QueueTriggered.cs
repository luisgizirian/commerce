using Microsoft.Extensions.Logging;
using Dapr.AzureFunctions.Extension;
using Microsoft.Azure.WebJobs;

namespace functions
{
    public class DaprQueueTriggered
    {
        private readonly ILogger _logger;

        public DaprQueueTriggered(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DaprQueueTriggered>();
        }

        [FunctionName("DaprQueueTriggered")]
        public void Run([DaprTopicTrigger("pubsub", Topic = "orders")] string data)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {data}");
        }
    }
}
