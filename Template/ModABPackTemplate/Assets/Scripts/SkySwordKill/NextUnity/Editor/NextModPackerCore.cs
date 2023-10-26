using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.NextUnity.Editor
{
    public class NextModPackerCore
    {
        public static void PackToAssetBundle(NextModPackerSetting packerSetting)
        {
            var rootDir = packerSetting.assetDir;
            var outDir = packerSetting.outDir;
            
            var builders = new List<AssetBundleBuild>();
            
            // 文件包
            var allAssets = RecursiveGetAllAssetsInDirectory(rootDir);
            if (allAssets.Count > 0)
            {
                var fileBuilder = new AssetBundleBuild();
                fileBuilder.assetBundleName = $"{packerSetting.bundleName}.ab";
            
                // 修正路径
                var assetRootFullPath = Application.dataPath;
                var allAssetsInRoot = allAssets
                    .Select(path =>
                    {
                        var fullPath = Path.GetFullPath(path);
                        var relativePath = fullPath.Substring(assetRootFullPath.Length).TrimStart(Path.DirectorySeparatorChar);
                        var mapperPath = Path.Combine("Assets/", relativePath).Replace("\\", "/");

                        return mapperPath;
                    });
                fileBuilder.assetNames = allAssetsInRoot.ToArray();
                
                // 修正地址
                var rootFullPath = Path.GetFullPath(rootDir);
                var allAssetsNameMapper = allAssets
                    .Select(path =>
                    {
                        var fullPath = Path.GetFullPath(path);
                        var relativePath = fullPath.Substring(rootFullPath.Length).TrimStart(Path.DirectorySeparatorChar);
                        var mapperPath = Path.Combine("Assets/", relativePath).Replace("\\", "/");

                        return mapperPath;
                    });
                fileBuilder.addressableNames = allAssetsNameMapper.ToArray();

                builders.Add(fileBuilder);
            }
            
            // 场景包
            var allScenes = Directory.EnumerateFiles(rootDir, "*.unity", SearchOption.AllDirectories).ToList();
            if (allScenes.Count > 0)
            {
                var sceneBuilder = new AssetBundleBuild();
                sceneBuilder.assetBundleName = $"{packerSetting.bundleName}_scene.ab";
                sceneBuilder.assetNames = allScenes.ToArray();
                sceneBuilder.addressableNames = allScenes.ToArray();
                builders.Add(sceneBuilder);
            }

            if (builders.Count == 0)
            {
                Debug.LogWarning($"没有找到任何资源，无法打包");
                return;
            }

            Directory.CreateDirectory(outDir);
            BuildPipeline.BuildAssetBundles(outDir, builders.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, 
                BuildTarget.StandaloneWindows64);
            Debug.Log($"打包完成，输出目录：{outDir}");
        }
        
        public static List<string> RecursiveGetAllAssetsInDirectory(string path)
        {
            List<string> assets = new List<string>();
            foreach (var f in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                if (Path.GetExtension(f) != ".meta" &&
                    Path.GetExtension(f) != ".cs" &&
                    Path.GetExtension(f) != ".unity")
                    assets.Add(f);
            return assets;
        }

        public static void TestLoadAssetBundle(NextModPackerSetting setting)
        {
            AssetBundle.UnloadAllAssetBundles(false);

            var fileAbPath = Path.Combine(setting.outDir, $"{setting.bundleName}.ab");
            if (!File.Exists(fileAbPath))
            {
                Debug.Log($"[测试加载] - {setting.bundleName} 文件包不存在");
            }
            else
            {
                var fileAb = AssetBundle.LoadFromFile(fileAbPath);
                if (fileAb == null)
                {
                    Debug.Log($"[测试加载] - 文件包加载失败");
                }
                else
                {
                    var allAssets = fileAb.GetAllAssetNames();
                    Debug.Log($"[测试加载] - 文件包加载成功，包含资源：{allAssets.Length} 个");
                    foreach (var asset in allAssets)
                    {
                        Debug.Log($"[测试加载] - 包含资源：{asset}");
                    }
                    fileAb.Unload(false);
                }
            }
            
            var sceneAbPath = Path.Combine(setting.outDir, $"{setting.bundleName}_scene.ab");
            if (!File.Exists(sceneAbPath))
            {
                Debug.Log($"[测试加载] - {setting.bundleName} 场景包不存在");
            }
            else
            {
                var sceneAb = AssetBundle.LoadFromFile(sceneAbPath);
                if (sceneAb == null)
                {
                    Debug.Log($"[测试加载] - 场景包加载失败");
                }
                else
                {
                    var allAssets = sceneAb.GetAllScenePaths();
                    Debug.Log($"[测试加载] - 场景包加载成功，包含场景：{allAssets.Length} 个");
                    foreach (var asset in allAssets)
                    {
                        Debug.Log($"[测试加载] 包含场景：{asset}");
                    }
                    
                    sceneAb.Unload(false);
                }
            }
            Debug.Log($"[测试加载] - {setting.bundleName} 测试加载完成");
            
            Resources.UnloadUnusedAssets();
            AssetBundle.UnloadAllAssetBundles(false);
        }
    }
}
