using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        private readonly ConcurrentQueue<IEnumerable<byte>> _bufferQueue;

        public CasinoSocket()
        {
            _socket = new ClientWebSocket();
            _encoder = new UTF8Encoding();

            _bufferQueue = new ConcurrentQueue<IEnumerable<byte>>();
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
                var buffer = new byte[1024];
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var trimmedBuffer = buffer.Take(result.Count);
                    _bufferQueue.Enqueue(trimmedBuffer);

                    if(result.EndOfMessage)
                    {
                        var builder = new StringBuilder();

                        while (_bufferQueue.TryDequeue(out var qBuffer))
                        {
                            builder.Append(_encoder.GetString(qBuffer.ToArray()));
                        }

                        await InternalMessageReceived(builder.ToString());
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}
