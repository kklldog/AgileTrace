using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AgileTrace.IService
{
    public class WebsocketClient
    {
        public string Id { get; set; }

        public WebSocket Client { get; set; }
    }

    public interface IWebsocketService
    {
        void SendToAll(string message);
        Task SendOne(WebsocketClient client, string message);
        void AddClient(WebsocketClient client);

        Task CloseClient(WebsocketClient client, WebSocketCloseStatus closeStatus, string closeDesc);
    }
}
