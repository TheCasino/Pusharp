using Pusharp.Utilities;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Pusharp.Entities.WebSocket;
using Pusharp.Models.WebSocket;
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

        /*
        private void FailedSend(string data, Exception ex)
        {
            await _client.InternalLogAsync(new LogMessage(LogLevel.Error, $"Failed to send: {ex}"));
        }

        private void SocketClosed(WebSocketCloseStatus reason)
        {
            await _client.InternalLogAsync(new LogMessage(LogLevel.Error, $"Websocket closed: {reason}"));
            _socket.Dispose(true);

            if (_attempts == RetryLimit)
            {
                await _client.InternalLogAsync(new LogMessage(LogLevel.Critical, $"{RetryLimit} failed attemps to connect... Terminating connection"));
                return;
            }
            
            await _client.InternalLogAsync(new LogMessage(LogLevel.Info, "Trying to restart WebSocket..."));
            _ = Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ =>
            {
                _attempts++;
                Connect();
            });
        }

        private void MessageReceived(string socketMessage)
        {
            var model = _serializer.ReadUtf8<MessageReceivedModel>(new Utf8String(socketMessage));
            var message = new WebSocketMessage(model);

            Task.Run(() => MessageReceivedAsync(message));
        }

        private async Task MessageReceivedAsync(WebSocketMessage message)
        {
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
                            _client.InternalPushReceivedAsync(new ReceivedPush(message.ReceivedModel));
                            break;

                        case "dismissal":
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Dismissed push"));
                            _client.InternalPushDismissedAsync(new DismissedPush(message.ReceivedModel));
                            break;

                        case "clip":
                            await _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Copy received"));
                            _client.InternalCopyReceivedAsync(new ReceivedCopy(message.ReceivedModel));
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

        private void StateChanged(WebSocketState newState, WebSocketState prevState)
        {
            if (newState == WebSocketState.Open)
            {
                await _client.InternalLogAsync(new LogMessage(LogLevel.Info, "WebSocket connection has been made"));
                _attempts = 0;
            }
        }*/
    }
}
