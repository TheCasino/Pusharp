using PureWebSockets;
using Pusharp.Entities.WebSocket;
using Pusharp.Models.WebSocket;
using Pusharp.Utilities;
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
        private readonly Logger _logger;

        private PureWebSocket _socket;

        private const int RetryLimit = 5;
        private int _attempts;

        public WebSocket(PushBulletClient client, PushBulletClientConfig config, JsonSerializer serializer, Logger logger)
        {
            _client = client;
            _config = config;
            _serializer = serializer;
            _logger = logger;
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
                _logger.InvokeLog(new LogMessage(LogLevel.Critical, exception.ToString()));
            }
            catch (WebSocketException exception)
            {
                _logger.InvokeLog(new LogMessage(LogLevel.Critical, exception.ToString()));
            }
        }

        private void FailedSend(string data, Exception ex)
        {
            _logger.InvokeLog(new LogMessage(LogLevel.Error, $"Failed to send: {ex}"));
        }

        private void SocketClosed(WebSocketCloseStatus reason)
        {
            _logger.InvokeLog(new LogMessage(LogLevel.Error, $"Websocket closed: {reason}"));
            _socket.Dispose(true);

            if (_attempts == RetryLimit)
            {
                _logger.InvokeLog(new LogMessage(LogLevel.Critical, $"{RetryLimit} failed attemps to connect... Terminating connection"));
                return;
            }
            
            _logger.InvokeLog(new LogMessage(LogLevel.Info, "Trying to restart WebSocket..."));
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
                    _logger.InvokeLog(new LogMessage(LogLevel.Debug, "Heartbeat received"));
                    break;
                case MessageType.PUSH:
                    _logger.InvokeLog(new LogMessage(LogLevel.Debug, "Push received"));

                    switch (message.ReceivedModel.Type)
                    {
                        case "mirror":
                            _logger.InvokeLog(new LogMessage(LogLevel.Debug, "Mirror push"));
                            _client.InternalPushReceivedAsync(new ReceivedPush(message.ReceivedModel));
                            break;

                        case "dismissal":
                            _logger.InvokeLog(new LogMessage(LogLevel.Debug, "Dismissed push"));
                            _client.InternalPushDismissedAsync(new DismissedPush(message.ReceivedModel));
                            break;

                        case "clip":
                            _logger.InvokeLog(new LogMessage(LogLevel.Debug, "Copy received"));
                            _client.InternalCopyReceivedAsync(new ReceivedCopy(message.ReceivedModel));
                            break;
                    }
                    break;

                case MessageType.TICKLE:
                    _logger.InvokeLog(new LogMessage(LogLevel.Debug, "Tickle received"));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Type));
            }
        }

        private void StateChanged(WebSocketState newState, WebSocketState prevState)
        {
            if (newState == WebSocketState.Open)
            {
                _logger.InvokeLog(new LogMessage(LogLevel.Info, "WebSocket connection has been made"));
                _attempts = 0;
            }
        }
    }
}
