using UnityEngine;

namespace SkySwordKill.Next
{
    public class BytesAsset : TextAsset
    {
        private byte[] _bytes;
        public byte[] Bytes => _bytes;
        
        public BytesAsset(byte[] bytes)
        {
            _bytes = bytes;
        }
    }
}