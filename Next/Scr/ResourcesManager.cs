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

        public bool TryGetAsset(string path,Action<Object> callback)
        {
            if (fileAssets.TryGetValue(path.ToLower(), out var fileAsset))
            {
                fileAsset.LoadAsset(callback);
                return true;
            }
            return false;
        }
        
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

        public void OnGUI()
        {
            if (isLoading)
            {
                GUI.Window(0, new Rect(0, 0, 300, 100), DrawWindow, "Next 资源加载中......");
            }
        }

        #endregion

        #region 私有方法

        private void DrawWindow(int id)
        {
            var assetCount = fileAssets.Count;
            var loadSuccessCount = fileAssets.Count(pair => pair.Value.IsDone);
            GUILayout.Label($"资源加载进度：{loadSuccessCount} / {assetCount}");

            if (assetCount == loadSuccessCount)
            {
                isLoading = false;
            }
        }

        #endregion


    }
}