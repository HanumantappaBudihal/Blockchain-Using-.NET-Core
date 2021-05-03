using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace PeerToPeerNetwork
{
    public class Client
    {
        IDictionary<string, WebSocket> webSocketDictionary = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!webSocketDictionary.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);

                webSocket.OnMessage += (senderEvent, sender) =>
                {
                    if (sender.Data == "Hi Client")
                    {
                        Console.WriteLine(sender.Data);
                    }
                    else
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(sender.Data);
                        
                        if (newChain.IsValid() && newChain.Chain.Count > Program.SimpleCoin.Chain.Count)
                        {
                            List<Transaction> newTransactions = new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransactions);
                            newTransactions.AddRange(Program.SimpleCoin.PendingTransactions);

                            newChain.PendingTransactions = newTransactions;
                            Program.SimpleCoin = newChain;
                        }
                    }
                };

                webSocket.Connect();
                webSocket.Send("Hi Server");
                webSocket.Send(JsonConvert.SerializeObject(Program.SimpleCoin));
                webSocketDictionary.Add(url, webSocket);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var webSocket in webSocketDictionary)
            {
                if (webSocket.Key == url)
                {
                    webSocket.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var webSocket in webSocketDictionary)
            {
                webSocket.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var webSocket in webSocketDictionary)
            {
                servers.Add(webSocket.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var webSocket in webSocketDictionary)
            {
                webSocket.Value.Close();
            }
        }
    }
}
