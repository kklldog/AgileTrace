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
        private static readonly ConcurrentDictionary<string, WebsocketClient> Clients = new ConcurrentDictionary<string, WebsocketClient>();

        public void SendToAll(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            foreach (var webSocket in Clients.Values)
            {
                webSocket.Client.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true,
                    CancellationToken.None);
            }
        }

        public async Task SendOne(WebsocketClient client, string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            await client.Client.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }

        public void AddClient(WebsocketClient client)
        {
            Clients.TryAdd(client.Id, client);
        }

        public async Task CloseClient(WebsocketClient client, WebSocketCloseStatus closeStatus, string closeDesc)
        {
            Clients.TryRemove(client.Id, out client);
            await client.Client.CloseAsync(closeStatus, closeDesc, CancellationToken.None);
            client.Client.Dispose();
        }
    }
}
