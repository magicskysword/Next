using System;

namespace SkySwordKill.Next.Mod;

public class ModOpenException : ModException
{
    public ModOpenException()
    {
    }

    public ModOpenException(string message) : base(message)
    {
    }

    public ModOpenException(string message, Exception inner) : base(message, inner)
    {
    }
}