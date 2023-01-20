using System;

namespace SkySwordKill.Next.Mod;

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
}

