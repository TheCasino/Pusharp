using System;
using Pusharp.Models.WebSocket;
using Model = Pusharp.Models.WebSocket.MessageReceivedModel;

namespace Pusharp.Entities.WebSocket
{
    internal class WebSocketMessage
    {
        private readonly Model _model;

        public MessageType Type
        {
            get
            {
                switch (_model.Type)
                {
                    case "nop":
                        return MessageType.HEARTBEAT;

                    case "tickle":
                        return MessageType.TICKLE;

                    case "push":
                        return MessageType.PUSH;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(_model.Type));
                }
            }
        }

        public SubType SubType
        {
            get
            {
                switch (Type)
                {
                    case MessageType.HEARTBEAT:
                        return SubType.NONE;

                    case MessageType.TICKLE:
                        switch (_model.SubType)
                        {
                            case "push":
                                return SubType.PUSH;

                            case "device":
                                return SubType.DEVICE;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(_model.SubType));
                        }

                    case MessageType.PUSH:
                        return SubType.NONE;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(Type));
                }
            }
        }

        public PushReceivedModel ReceivedModel => _model.Push;

        public WebSocketMessage(Model model)
        {
            _model = model;
        }
    }
}
