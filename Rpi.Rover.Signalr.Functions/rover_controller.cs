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
        enum MotorControl
        {
            Stop, Forward, LeftForward, RightForward, LeftBackward, RightBackward, Backward, SharpLeft, SharpRight, ShutDown, Unknown
        }

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
            [SignalRConnectionInfo(HubName = "Rover")]SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("stop")]
        public static Task Stop(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.Stop).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("forward")]
        public static Task Forward(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.Forward).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("leftforward")]
        public static Task LeftForward(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.LeftForward).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("rightforward")]
        public static Task RightForward(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.RightForward).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("leftbackward")]
        public static Task LeftBackward(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.LeftBackward).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("rightbackward")]
        public static Task RightBackward(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.RightBackward).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("backward")]
        public static Task Backward(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.Backward).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("sharpleft")]
        public static Task SharpLeft(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.SharpLeft).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("sharpright")]
        public static Task SharpRight(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.SharpRight).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }

        [FunctionName("shutDown")]
        public static Task ShutDown(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [SignalR(HubName = "ROVER")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string command = ((int)MotorControl.ShutDown).ToString();

            return signalRMessages.AddAsync(
                   new SignalRMessage
                   {
                       Target = "newMessage",
                       Arguments = new[] { command }
                   });
        }
    }
}
