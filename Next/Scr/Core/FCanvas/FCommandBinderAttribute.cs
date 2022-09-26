using System;

namespace SkySwordKill.Next.FCanvas;

public class FCommandBinderAttribute : Attribute
{
    public Type Type;
        
    public FCommandBinderAttribute(Type bindType)
    {
        Type = bindType;
    }
}