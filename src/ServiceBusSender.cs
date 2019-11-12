using ClaimHandlingApp.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimHandlingApp
{
    public class ServiceBusSender
    {
        private readonly TopicClient _topicClient;
        private readonly IConfiguration _configuration;
        private const string TopicName = "testtopic";

        public ServiceBusSender(IConfiguration configuration)
        {
            _configuration = configuration;
            var serviceBusConnectionString = _configuration.GetSection("CosmosDb").GetSection("ServiceBusConnectionString").Value;           
            _topicClient = new TopicClient(
              serviceBusConnectionString,
              TopicName);
        }

        public async Task SendMessage(ClaimAudit payload)
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            await _topicClient.SendAsync(message);
        }
    }
}
