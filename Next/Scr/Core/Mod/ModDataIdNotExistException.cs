using System;

namespace SkySwordKill.Next.Mod;

public class ModDataIdNotExistException : ModException
{
    public ModDataIdNotExistException()
    {
    }

    public ModDataIdNotExistException(string message) : base(message)
    {
    }

    public ModDataIdNotExistException(string message, Exception inner) : base(message, inner)
    {
    }
}