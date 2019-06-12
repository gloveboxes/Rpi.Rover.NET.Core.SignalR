using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Rpi.Rover.Server
{
    class Program
    {
        enum MotorMap : byte
        {
            TwoPlus = 21, TwoMinus = 26, OnePlus = 19, OneMinus = 20
        }

        enum MotorControl
        {
            Stop, Forward, LeftForward, RightForward, LeftBackward, RightBackward, Backward, SharpLeft, SharpRight, ShutDown, Unknown
        }

        static GpioController controller = new GpioController();
        static Motor left = new Motor(controller, (int)MotorMap.TwoMinus, (int)MotorMap.TwoPlus);
        static Motor right = new Motor(controller, (int)MotorMap.OneMinus, (int)MotorMap.OnePlus);

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Expecting Rover Controller SignalR Azure Function URI as command line argument");
            }
            else
            {
                Console.WriteLine(args[0]);
            }

            Uri signalrFunctionUri = new Uri(args[0]);

            var signalrConnection = new HubConnectionBuilder()
                 .WithUrl(signalrFunctionUri)
                 .ConfigureLogging(logging =>
                 {
                     logging.SetMinimumLevel(LogLevel.Information);
                     logging.AddConsole();
                 }).Build();

            signalrConnection.On<string>("newMessage", RoverActions);

            signalrConnection.Closed += async e =>
            {
                Console.WriteLine("### SignalR Connection closed... ###");
                await signalrConnection.StartAsync();
                Console.WriteLine("### Connected to SignalR... ###");
            };

            // wrap in exception handler in case no current active internet connection
            try
            {
                await signalrConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Thread.Sleep(Timeout.Infinite);
        }

        static void RoverActions(string action)
        {
            int cmd;

            if (int.TryParse(action, out cmd))
            {
                switch ((MotorControl)cmd)
                {
                    case MotorControl.Stop: // stop
                        left.Stop();
                        right.Stop();
                        break;
                    case MotorControl.Forward: // forward
                        left.Forward();
                        right.Forward();
                        break;
                    case MotorControl.LeftForward: // left
                        left.Stop();
                        right.Forward();
                        break;
                    case MotorControl.RightForward: // right
                        left.Forward();
                        right.Stop();
                        break;
                    case MotorControl.LeftBackward: // leftbackward
                        left.Stop();
                        right.Backward();
                        break;
                    case MotorControl.RightBackward: // right backward
                        left.Backward();
                        right.Stop();
                        break;
                    case MotorControl.Backward:
                        left.Backward();
                        right.Backward();
                        break;
                    case MotorControl.SharpLeft: // sharpleft
                        left.Forward();
                        right.Backward();
                        break;
                    case MotorControl.SharpRight: //sharpright
                        left.Backward();
                        right.Forward();
                        break;
                    case MotorControl.ShutDown:
                        ShutDown();
                        break;
                }
            }
        }

        static void ShutDown()
        {
            string result;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"/bin/bash";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.Arguments = "-c \"sudo halt\"";

            try
            {
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
