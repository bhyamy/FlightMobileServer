using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlightMobileApp.Models
{
    public class FlightGearClient:IClient
    {
        private readonly BlockingCollection<AsyncCommand> _queue;
        private readonly TcpClient _client;
        private readonly Dictionary<string,string> _addressDictionary;

        public FlightGearClient(IConfiguration configuration)
        {
            string ip = configuration.GetValue<string>("Logging:ServerInfo:ip");
            int controllPort = configuration.GetValue<int>("Logging:ServerInfo:controll_port");
            _queue = new BlockingCollection<AsyncCommand>();
            _client = new TcpClient();
            Start();

            _client.Connect(ip, controllPort);
            NetworkStream stream = _client.GetStream();
            Byte[] sendBuffer = Encoding.ASCII.GetBytes("data\n");
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            stream.Flush();
            _addressDictionary = new Dictionary<string, string>
            {
                {"Aileron", "/controls/flight/aileron"},
                {"Rudder", "/controls/flight/rudder"},
                {"Elevator", "/controls/flight/elevator"},
                {"Throttle", "/controls/engines/current-engine/throttle"}
            };
        }

        // Called by the WebApi Controller, it will await on the returned Task<>
        // This is not an async method, since it does not await anything.
        public async Task<Result> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            _queue.Add(asyncCommand);
            return await asyncCommand.Task;
        }

        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }
        public void ProcessCommands()
        {
            foreach (AsyncCommand command in _queue.GetConsumingEnumerable())
            {
                bool messageReceived = true;
                foreach (PropertyInfo property in command.Command.GetType().GetProperties())
                {
                    messageReceived = messageReceived && SendMessage(property, command.Command);
                }
                
                Result res = messageReceived? Result.Ok : Result.NotOk;
                    // TaskCompletionSource allows an external thread to set
                    // the result (or the exceptino) on the associated task object
                command.Completion.SetResult(res);
            }
        }

        public bool SendMessage(PropertyInfo prop, Command cmd)
        {
            NetworkStream stream = _client.GetStream();
            float val = Convert.ToSingle(prop.GetValue(cmd, null));
            // sending message
            string message = "set" + " " + _addressDictionary[prop.Name] + " " + val + "\n";
            Byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            message = "get" + " " + _addressDictionary[prop.Name] + " " + val + "\n";
            sendBuffer = Encoding.ASCII.GetBytes(message);
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            // receiving answer
            Byte[] recvBuffer = new Byte[1024];
            int nRead = stream.Read(recvBuffer, 0, 1024);
            stream.Flush();
            string responseData = Encoding.ASCII.GetString(recvBuffer, 0, nRead);
            float temp = float.Parse(responseData);
            return (val + 0.01 > temp && val - 0.01 < temp);
        }
    }
}
