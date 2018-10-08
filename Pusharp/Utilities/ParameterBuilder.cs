using System;
using System.Collections.Generic;

namespace Pusharp.Utilities
{
    internal class ParameterBuilder
    {
        private readonly Dictionary<string, bool> _requirements = new Dictionary<string, bool>();

        internal ParameterBuilder AddRequirement(string name, bool isTrue)
        {
            _requirements[name] = isTrue;
            return this;
        }

        internal void ValidateParameters()
        {
            foreach (var req in _requirements)
                if (!req.Value)
                    throw new ParameterFailedException(req.Key);
        }
    }

    internal class ParameterFailedException : Exception
    {
        internal ParameterFailedException(string reason) : base("The following requirement failed: " + reason)
        {
        }
    }
}