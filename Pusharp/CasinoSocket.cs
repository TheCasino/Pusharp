using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pusharp
{
    internal class CasinoSocket
    {
        private readonly ClientWebSocket _socket;
        private readonly UTF8Encoding _encoder;

        public CasinoSocket()
        {
            _socket = new ClientWebSocket();
            _encoder = new UTF8Encoding();
        }

        public async Task ConnectAsync(string url)
        {
            var uri = new Uri(url);
            await _socket.ConnectAsync(uri, CancellationToken.None);

            Task.Run(async () => await Receiving());
        }

        public event Func<string, Task> MessageReceived;

        private Task InternalMessageReceived(string message)
        {
            return MessageReceived is null ? Task.CompletedTask : MessageReceived.Invoke(message);
        }

        private async Task Receiving()
        {
            while (_socket.State == WebSocketState.Open)
            {
                using (var stream = new MemoryStream())
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        var buffer = new byte[1024];
                        result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        switch (result.MessageType)
                        {
                            case WebSocketMessageType.Text:
                                await stream.WriteAsync(buffer, 0, result.Count);
                                break;

                            default:
                                break;
                        }
                    }
                    while (!result.EndOfMessage);

                    var bytes = stream.ToArray();
                    if(bytes.Length <= 0)
                        break;

                    await InternalMessageReceived(_encoder.GetString(bytes));
                }                
            }
        }
    }
}
