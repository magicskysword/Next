using System;
using System.Runtime.Serialization;

namespace SkySwordKill.Next.Mod
{
    public class ModLoadException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ModLoadException()
        {
        }

        public ModLoadException(string message) : base(message)
        {
        }

        public ModLoadException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ModLoadException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}