using System;

namespace Pusharp.Entities
{
    public class File
    {
        public string Name { get; }
        public string Type { get; }
        public Uri Url { get; }

        internal File(string name, string type, string url)
        {
            Name = name;
            Type = type;
            Url = new Uri(url);
        }
    }
}
