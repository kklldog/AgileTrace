using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using AgileTrace.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;

namespace AgileTrace.Middleware
{
    public class WebSocketHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebsocketService _websocketService;

        public WebSocketHandlerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,IWebsocketService websocketService)
        {
            _next = next;
            _logger = loggerFactory.
                CreateLogger<WebSocketHandlerMiddleware>();
            _websocketService = websocketService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var client = new WebsocketClient()
                    {
                        Client = webSocket,
                        Id = Guid.NewGuid().ToString()
                    };
                    _websocketService.AddClient(client);
                    await Echo(context, client);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Echo(HttpContext context, WebsocketClient webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.Client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.Client.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.Client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            _logger.LogInformation($"websocket close , closeStatus:{webSocket.Client.CloseStatus} closeDesc:{webSocket.Client.CloseStatusDescription}");
            await _websocketService.CloseClient(webSocket,result.CloseStatus.Value, result.CloseStatusDescription);
        }
    }
}
