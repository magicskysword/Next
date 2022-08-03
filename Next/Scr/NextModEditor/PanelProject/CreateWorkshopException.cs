using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace SkySwordKill.NextEditor.PanelProject
{
    public class CreateWorkshopException : Exception
    {
        public CreateWorkshopException()
        {
        }

        public CreateWorkshopException(string message) : base(message)
        {
        }

        public CreateWorkshopException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CreateWorkshopException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}