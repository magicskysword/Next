using System;

namespace SkySwordKill.Next.FCanvas
{
    public class PCommandBinderAttribute : Attribute
    {
        public string BindType;
        
        public PCommandBinderAttribute(string bindType)
        {
            BindType = bindType;
        }
    }
}