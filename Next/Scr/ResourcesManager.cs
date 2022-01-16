using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next
{
    public class ResourcesManager : MonoBehaviour
    {
        #region 字段

        public Dictionary<string, FileAsset> fileAssets = new Dictionary<string, FileAsset>();
        public Dictionary<int, Sprite> spriteCache = new Dictionary<int, Sprite>();

        private bool isLoading = false;
        
        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public void Init()
        {
            fileAssets.Clear();
            spriteCache.Clear();
        }

        /// <summary>
        /// 异步加载资源，返回值表示资源是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool TryGetAsset(string path,Action<Object> callback)
        {
            if (fileAssets.TryGetValue(path.ToLower(), out var fileAsset))
            {
                fileAsset.LoadAsset(callback);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 同步加载资源，返回值表示资源是否存在且加载完毕
        /// </summary>
        /// <param name="path"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        public bool TryGetAsset(string path,out Object asset)
        {
            if (fileAssets.TryGetValue(path.ToLower(), out var fileAsset)
                && fileAsset.IsDone)
            {
                asset = fileAsset.asset;
                return true;
            }
            asset = null;
            return false;
        }

        /// <summary>
        /// 异步按文件夹加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <param name="includeSubfolder">是否包含子文件夹</param>
        /// <param name="extension">文件拓展名</param>
        /// <returns></returns>
        public void TryGetAssets(string path,Action<Object[]> callback,bool includeSubfolder = false,string extension = ".*")
        {
            var list = new List<FileAsset>();
            var transPath = path.ToLower();
            foreach (var pair in fileAssets)
            {
                var curPath = pair.Key;
                var asset = pair.Value;
                if (curPath.StartsWith(transPath)
                    && (includeSubfolder || !curPath.Substring(transPath.Length).Contains("/")) 
                    && (extension == ".*" || curPath.EndsWith(extension)))
                {
                    list.Add(asset);
                }
            }

            StartCoroutine(LoadAssets(list,callback));
        }
        
        /// <summary>
        /// 同步按文件夹加载资源（不推荐）
        /// 如果一个资源没有准备就绪，不会被加载进来
        /// </summary>
        /// <param name="path"></param>
        /// <param name="asstes"></param>
        /// <param name="includeSubfolder"></param>
        /// <param name="extension"></param>
        public void TryGetAssets(string path,out Object[] asstes,bool includeSubfolder = false,string extension = ".*")
        {
            var list = new List<FileAsset>();
            var transPath = path.ToLower();
            foreach (var pair in fileAssets)
            {
                var curPath = pair.Key;
                var asset = pair.Value;
                if (curPath.StartsWith(transPath)
                    && (includeSubfolder || !curPath.Substring(transPath.Length).Contains("/")) 
                    && (extension == ".*" || curPath.EndsWith(extension)))
                {
                    list.Add(asset);
                }
            }

            asstes = list
                .Where(asset=>asset.IsDone)
                .Select(asset=>asset.asset)
                .ToArray();
        }

        private IEnumerator LoadAssets(IEnumerable<FileAsset> assets,Action<Object[]> callback)
        {
            var loadAssets = new List<FileAsset>();
            var doneAssets = new List<FileAsset>();
            
            loadAssets.AddRange(assets);
            
            while (doneAssets.Count < loadAssets.Count)
            {
                for (int i = loadAssets.Count - 1; i >= 0; i--)
                {
                    var asset = loadAssets[i];
                    if (asset.IsDone)
                    {
                        loadAssets.RemoveAt(i);
                        doneAssets.Add(asset);
                    }
                }

                yield return null;
            }
            
            callback?.Invoke(doneAssets.Select(asset=>asset.asset).ToArray());
        }

        /// <summary>
        /// 是否存在资源（无论是否加载完毕）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool HaveAsset(string path)
        {
            return fileAssets.ContainsKey(path.ToLower());
        }
        
        /// <summary>
        /// 是否存在资源且加载完毕
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsAssetReady(string path)
        {
            return fileAssets.TryGetValue(path.ToLower(), out var fileAsset) && fileAsset.IsDone;
        }

        public void AddAsset(string cachePath,string path)
        {
            var fileAsset = new FileAsset()
            {
                filePath = path
            };
            var key = cachePath.Replace(@"\\", "/").ToLower();
            
            if (fileAssets.TryGetValue(key, out var oldAsset))
            {
                Main.LogWarning($"重复添加Asset ({key})");
            }
            else
            {
                Main.LogInfo($"添加Asset ({key})");
            }
            fileAssets[key] = fileAsset;
        }

        public Sprite GetSpriteCache(Texture2D texture)
        {
            if(!spriteCache.TryGetValue(texture.GetHashCode(),out var sprite))
            {
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                spriteCache.Add(texture.GetHashCode(),sprite);
            }
            return sprite;
        }

        public void StartLoadAsset()
        {
            isLoading = true;
            foreach (var fileAsset in fileAssets)
            {
                StartCoroutine(fileAsset.Value.StartLoad());
            }
        }

        private void Update()
        {
            if (isLoading)
            {
                var assetCount = fileAssets.Count;
                var loadSuccessCount = fileAssets.Count(pair => pair.Value.IsDone);
                if (assetCount == loadSuccessCount)
                {
                    isLoading = false;
                }
            }
        }

        public void OnGUI()
        {
            if (isLoading)
            {
                GUILayout.Window(0, new Rect(0, 0, 300, 100), DrawWindow, "Next 资源加载中......");
            }
        }

        #endregion

        #region 私有方法

        private void DrawWindow(int id)
        {
            var assetCount = fileAssets.Count;
            var loadSuccessCount = fileAssets.Count(pair => pair.Value.IsDone);
            GUILayout.Label($"资源加载进度：{loadSuccessCount} / {assetCount}");
        }

        #endregion


    }
}