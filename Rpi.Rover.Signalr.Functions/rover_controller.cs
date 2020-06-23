using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Rpi.Rover.Signalr.Functions
{
    public static class rover_controller
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
            [SignalRConnectionInfo(HubName = "Rover")]SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("control")]
        public static Task Control(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = req.Query["cmd"];

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }
    }
}
