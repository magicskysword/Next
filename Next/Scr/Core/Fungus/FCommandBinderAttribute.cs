using System;

namespace SkySwordKill.Next.FungusTools
{
    public class FCommandBinderAttribute : Attribute
    {
        public Type Type;
        
        public FCommandBinderAttribute(Type bindType)
        {
            Type = bindType;
        }
    }
}