using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next
{
    public class FileAsset
    {
        #region 字段

        public Object asset;
        public event Action<Object> OnLoadedAsset;
        public string filePath;

        private bool isDone = false;
        private bool isLoaded = false;

        #endregion

        #region 属性

        public bool IsDone => isDone;
        public bool IsLoaded => isLoaded;

        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public IEnumerator StartLoad()
        {
            if(isLoaded)
                yield break;
            isLoaded = true;
            var extension = Path.GetExtension(filePath);
            switch (extension)
            {
                case ".jpg":
                case ".png":
                    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(filePath))
                    {
                        yield return webRequest.SendWebRequest();
                        if(webRequest.isDone)
                        {
                            asset = DownloadHandlerTexture.GetContent(webRequest);
                            OnLoadedAsset?.Invoke(asset);
                        }
                    }
                    break;
                case ".ab":
                    using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(filePath))
                    {
                        yield return webRequest.SendWebRequest();
                        if(webRequest.isDone)
                        {
                            var ab = DownloadHandlerAssetBundle.GetContent(webRequest);
                            asset = ab;
                            OnLoadedAsset?.Invoke(asset);
                        }
                    }
                    break;
            }
            isDone = true;
        }

        public void LoadAsset(Action<Object> callback)
        {
            if (!isDone)
            {
                OnLoadedAsset += callback;
            }
            else
            {
                callback?.Invoke(asset);
            }
        }

        #endregion

        #region 私有方法



        #endregion


    }
}