using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgileTrace.Services
{
    public class WebsocketService
    {
        private static readonly ConcurrentBag<WebSocket> Clients = new ConcurrentBag<WebSocket>();

        public static void SendToAll(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            foreach (var webSocket in Clients)
            {
                webSocket.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true,
                    CancellationToken.None);
            }
        }

        public static async Task SendOne(WebSocket client, string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }

        public static void AddClient(WebSocket client)
        {
            Clients.Add(client);
        }

        public static async Task CloseClient(WebSocket client, WebSocketCloseStatus closeStatus, string closeDesc)
        {
            Clients.TryTake(out client);
            await client.CloseAsync(closeStatus, closeDesc, CancellationToken.None);
            client.Dispose();
        }
    }
}
