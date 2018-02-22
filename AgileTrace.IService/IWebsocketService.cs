using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AgileTrace.IService
{
    public interface IWebsocketService
    {
        void SendToAll(string message);
        Task SendOne(WebSocket client, string message);
        void AddClient(WebSocket client);

        Task CloseClient(WebSocket client, WebSocketCloseStatus closeStatus, string closeDesc);
    }
}
