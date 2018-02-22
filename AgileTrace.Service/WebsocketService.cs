using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgileTrace.IService;

namespace AgileTrace.Service
{
    public class WebsocketService : IWebsocketService
    {
        private static readonly ConcurrentBag<WebSocket> Clients = new ConcurrentBag<WebSocket>();

        public void SendToAll(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            foreach (var webSocket in Clients)
            {
                webSocket.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true,
                    CancellationToken.None);
            }
        }

        public async Task SendOne(WebSocket client, string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }

        public void AddClient(WebSocket client)
        {
            Clients.Add(client);
        }

        public async Task CloseClient(WebSocket client, WebSocketCloseStatus closeStatus, string closeDesc)
        {
            Clients.TryTake(out client);
            await client.CloseAsync(closeStatus, closeDesc, CancellationToken.None);
            client.Dispose();
        }
    }
}
