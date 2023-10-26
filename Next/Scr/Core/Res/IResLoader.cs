using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Res;

public interface IResLoader
{
    void Reset();
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="resPath"></param>
    /// <returns></returns>
    Object Load(string resPath);
    
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="resPath"></param>
    /// <returns></returns>
    UniTask<Object> LoadAsync(string resPath);
    
    /// <summary>
    /// 加载字节流
    /// </summary>
    /// <param name="resPath"></param>
    /// <returns></returns>
    byte[] LoadBytes(string resPath);
    
    /// <summary>
    /// 异步加载字节流
    /// </summary>
    /// <param name="resPath"></param>
    /// <returns></returns>
    UniTask<byte[]> LoadBytesAsync(string resPath);

    string LoaderCode { get; }
}