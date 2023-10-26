using System;
using UnityEngine.Serialization;

namespace SkySwordKill.NextUnity
{
    [Serializable]
    public class NextModPackerSetting
    {
        /// <summary>
        /// AB包名
        /// </summary>
        public string bundleName;
        
        /// <summary>
        /// 文件目录，映射到 Assets/ 下
        /// </summary>
        public string assetDir;
        
        /// <summary>
        /// 打包输出目录
        /// </summary>
        public string outDir;
    }
}