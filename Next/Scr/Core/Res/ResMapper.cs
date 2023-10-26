using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Res;

public readonly struct ResMapper
{
    public readonly string resPath;
    public readonly IResLoader resLoader;
    
    public ResMapper(string resPath, IResLoader resLoader)
    {
        this.resPath = resPath;
        this.resLoader = resLoader;
    }
    
    public Object Load()
    {
        return resLoader.Load(resPath);
    }
    
    public async UniTask<Object> LoadAsync()
    {
        return await resLoader.LoadAsync(resPath);
    }

    public byte[] LoadBytes()
    {
        return resLoader.LoadBytes(resPath);
    }

    public async Task<byte[]> LoadBytesAsync()
    {
        return await resLoader.LoadBytesAsync(resPath);
    }
}