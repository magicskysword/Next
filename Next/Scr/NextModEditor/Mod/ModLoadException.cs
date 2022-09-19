using System;
using System.Runtime.Serialization;

namespace SkySwordKill.NextModEditor.Mod
{
    [Serializable]
    public class ModException : Exception
    {
        public ModException()
        {
        }

        public ModException(string message) : base(message)
        {
        }

        public ModException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ModException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}