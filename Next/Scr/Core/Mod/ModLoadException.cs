using System;

namespace SkySwordKill.Next.Mod;

public class ModLoadException : Exception
{
    public ModLoadException()
    {
    }

    public ModLoadException(string message) : base(message)
    {
    }

    public ModLoadException(string message, Exception inner) : base(message, inner)
    {
    }
}