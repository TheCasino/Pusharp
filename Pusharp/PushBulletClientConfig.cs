namespace Pusharp
{
    public class PushBulletClientConfig
    {
        public string ApiBaseUrl { get; set; } = "https://api.pushbullet.com";
        public string WebSocketUrl { get; set; } = "wss://stream.pushbullet.com/websocket/";
        public string Token { get; set; }
    }
}