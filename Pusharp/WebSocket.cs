using PureWebSockets;
using Pusharp.Entities.WebSocket;
using Pusharp.Models.WebSocket;
using Pusharp.Utilities;
using System;
using System.Net.Http.Headers;
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

        private PureWebSocket _socket;

        private const int RetryLimit = 5;
        private int _attempts;

        public WebSocket(PushBulletClient client, PushBulletClientConfig config, JsonSerializer serializer)
        {
            _client = client;
            _config = config;
            _serializer = serializer;
        }

        public void Connect()
        {
            var options = new PureWebSocketOptions
            {
                SendDelay = 100,
                IgnoreCertErrors = true
            };

            _socket = new PureWebSocket($"{_config.WebSocketUrl}/{_config.Token}", options);

            _socket.OnStateChanged += StateChanged;
            _socket.OnMessage += MessageReceived;
            _socket.OnClosed += SocketClosed;
            _socket.OnSendFailed += FailedSend;

            try
            {
                _socket.Connect();
            }
            catch (AggregateException exception)
            {
                _client.InternalLogAsync(new LogMessage(LogLevel.Critical, exception.ToString()));
            }
            catch (WebSocketException exception)
            {
                _client.InternalLogAsync(new LogMessage(LogLevel.Critical, exception.ToString()));
            }
        }

        private void FailedSend(string data, Exception ex)
        {
            _client.InternalLogAsync(new LogMessage(LogLevel.Error, $"Failed to send: {ex}"));
        }

        private void SocketClosed(WebSocketCloseStatus reason)
        {
            _client.InternalLogAsync(new LogMessage(LogLevel.Error, $"Websocket closed: {reason}"));
            _socket.Dispose(true);

            if (_attempts == RetryLimit)
            {
                _client.InternalLogAsync(new LogMessage(LogLevel.Critical, $"{RetryLimit} failed attemps to connect... Terminating connection"));
                return;
            }
            
            _client.InternalLogAsync(new LogMessage(LogLevel.Info, "Trying to restart WebSocket..."));
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

            switch (message.Type)
            {
                case MessageType.HEARTBEAT:
                    _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Heartbeat received"));
                    break;
                case MessageType.PUSH:
                    _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Push received"));

                    switch (message.ReceivedModel.Type)
                    {
                        case "mirror":
                            _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Mirror push"));
                            _client.InternalPushReceivedAsync(new ReceivedPush(message.ReceivedModel));
                            break;

                        case "dismissal":
                            _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Dismissed push"));
                            _client.InternalPushDismissedAsync(new DismissedPush(message.ReceivedModel));
                            break;
                    }


                    break;

                case MessageType.TICKLE:
                    _client.InternalLogAsync(new LogMessage(LogLevel.Debug, "Tickle received"));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Type));
            }
        }

        private void StateChanged(WebSocketState newState, WebSocketState prevState)
        {
            if (newState == WebSocketState.Open)
            {
                _client.InternalLogAsync(new LogMessage(LogLevel.Info, "WebSocket connection has been made"));
                _attempts = 0;
            }
        }
    }
}
