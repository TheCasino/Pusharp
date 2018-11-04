using Pusharp.Entities.WebSocket;
using Pusharp.Models.WebSocket;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Voltaic;
using Voltaic.Serialization.Json;

namespace Pusharp
{
    internal class WebSocket
    {
        private readonly PushBulletClient _client;
        private readonly PushBulletClientConfig _config;
        private readonly JsonSerializer _serializer;

        private readonly CasinoSocket _socket;
        
        public WebSocket(PushBulletClient client, PushBulletClientConfig config, JsonSerializer serializer)
        {
            _socket = new CasinoSocket();
            _socket.MessageReceived += MessageReceived;

            _client = client;
            _config = config;
            _serializer = serializer;
        }

        public async Task ConnectAsync()
        {
            try
            {
                await _socket.ConnectAsync($"{_config.WebSocketUrl}{_config.Token}");
            }
            catch (AggregateException exception)
            {
                await _client.InternalLogAsync(new LogMessage(LogLevel.Critical, exception.ToString()));
            }
            catch (WebSocketException exception)
            {
                await _client.InternalLogAsync(new LogMessage(LogLevel.Critical, exception.ToString()));
            }
        }

        private async Task MessageReceived(string socketMessage)
        {
            var model = _serializer.ReadUtf8<MessageReceivedModel>(new Utf8String(socketMessage));
            var message = new WebSocketMessage(model);

            switch (message.Type)
            {
                case MessageType.HEARTBEAT:
                    await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Heartbeat received"));
                    break;
                case MessageType.PUSH:
                    await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Push received"));

                    switch (message.ReceivedModel.Type)
                    {
                        case "mirror":
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Mirror push"));
                            await _client.InternalPushReceivedAsync(new ReceivedPush(message.ReceivedModel));
                            break;

                        case "dismissal":
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Dismissed push"));
                            await _client.InternalPushDismissedAsync(new DismissedPush(message.ReceivedModel));
                            break;

                        case "clip":
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Copy received"));
                            await _client.InternalCopyReceivedAsync(new ReceivedCopy(message.ReceivedModel));
                            break;
                    }
                    break;

                case MessageType.TICKLE:
                    await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Tickle received"));

                    switch (message.SubType)
                    {
                        case SubType.DEVICE:
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Device updated"));
                            await _client.UpdateDeviceCacheAsync();
                            break;

                        case SubType.PUSH:
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Push updated"));
                            await _client.UpdatePushCacheAsync();
                            break;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Type));
            }
        }
    }
}
