using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace PeerToPeerNetwork
{
    public class Server : WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer webSocketServer = null;

        public void Start()
        {
            webSocketServer = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            webSocketServer.AddWebSocketService<Server>("/Blockchain");
            webSocketServer.Start();
            
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
        }

        protected override void OnMessage(MessageEventArgs eventArgs)
        {
            if (eventArgs.Data == "Hi Server")
            {
                Console.WriteLine(eventArgs.Data);
                Send("Hi Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(eventArgs.Data);

                if (newChain.IsValid() && newChain.Chain.Count > Program.SimpleCoin.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.SimpleCoin.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.SimpleCoin = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.SimpleCoin));
                    chainSynched = true;
                }
            }
        }
    }
}
