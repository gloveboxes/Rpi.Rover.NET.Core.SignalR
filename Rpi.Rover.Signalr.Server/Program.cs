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
        enum MotorMap : byte { TwoPlus = 21, TwoMinus = 26, OnePlus = 19, OneMinus = 20 }
        enum MotorControl { Stop, Forward, LeftForward, RightForward, Backward, SharpLeft, SharpRight, ShutDown }

        static GpioController controller = new GpioController();
        static Motor left = new Motor(controller, (int)MotorMap.TwoMinus, (int)MotorMap.TwoPlus);
        static Motor right = new Motor(controller, (int)MotorMap.OneMinus, (int)MotorMap.OnePlus);

        static Action[][] direction = new Action[][]{
            new Action[] { left.Stop, right.Stop },         // stop
            new Action[] { left.Forward, right.Forward },   // forward
            new Action[] { left.Stop, right.Forward },      // left
            new Action[] { left.Forward, right.Stop },      // right
            new Action[] { left.Backward, right.Backward},  // backwards
            new Action[] { left.Forward, right.Backward },  // left circle
            new Action[] { left.Backward, right.Forward },  // right circle
            new Action[] { ShutDown, null}                  // shutdown
        };

        static HubConnection signalrConnection = null;

        static async Task Main(string[] args)
        {
            Uri signalrFunctionUri = new Uri(Environment.GetEnvironmentVariable("SIGNALR_URL"));

            signalrConnection = new HubConnectionBuilder()
                 .WithUrl(signalrFunctionUri)
                 .ConfigureLogging(logging =>
                 {
                     logging.SetMinimumLevel(LogLevel.Information);
                     logging.AddConsole();
                 }).Build();

            signalrConnection.On<string>("newMessage", RoverActions);
            signalrConnection.Closed += async e => await RestartSignalR();
            await StartSignalR();

            Thread.Sleep(Timeout.Infinite);
        }

        static void RoverActions(string action)
        {
            MotorControl control;

            if (Enum.TryParse(action, true, out control))
            {
                direction[(int)control][0]();
                direction[(int)control][1]();
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

        static async Task StartSignalR()
        {
            // wrap in exception handler in case no current active internet connection
            while (true)
            {
                try
                {
                    await signalrConnection.StartAsync();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(2000);
                }
            }
        }

        static async Task RestartSignalR()
        {
            Console.WriteLine("### SignalR Connection closed... ###");
            try
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await signalrConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"### SignalR Connection Exception: {ex.Message}");
            }
            Console.WriteLine("### Connected to SignalR... ###");
        }
    }
}
