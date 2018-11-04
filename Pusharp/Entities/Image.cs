using System;

namespace Pusharp.Entities
{
    public sealed class Image
    {
        public Uri Url { get; }
        public int Width { get; }
        public int Height { get; }

        internal Image(string url, int width, int height)
        {
            Url = new Uri(url);
            Width = width;
            Height = height;
        }
    }
}
