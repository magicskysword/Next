// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using FairyGUI;
// using UnityEngine;
// using UnityEngine.Networking;
//
// namespace SkySwordKill.Next
// {
//     public static class UIManager
//     {
//         public static Dictionary<string, UIPackage> packages = new Dictionary<string, UIPackage>();
//         public static Dictionary<string, ScriptWindow> windows = new Dictionary<string, ScriptWindow>();
//
//         public static void Init()
//         {
//             packages.Clear();
//             windows.Clear();
//             UIPackage.RemoveAllPackages();
//             GRoot.inst.SetContentScaleFactor(1920,1080);
//         }
//
//         public static void AddPackage(string assetPath,Action callback)
//         {
//             if(!packages.ContainsKey(assetPath))
//             {
//                 string packagePath = string.Empty;
//                 string modUIPath = string.Empty;
//                 foreach (var modConfig in ModManager.modConfigs)
//                 {
//                     string curPath = Path.Combine(modConfig.Path, "UIRes");
//                     string tagPath = Path.Combine(curPath, $"{assetPath}_fui.bytes");
//                     if (File.Exists(tagPath))
//                     {
//                         modUIPath = curPath;
//                         packagePath = tagPath;
//                         break;
//                     }
//                 }
//
//                 if (string.IsNullOrEmpty(modUIPath))
//                 {
//                     Main.LogError($"UI资源 {assetPath} 不存在");
//                     return;
//                 }
//                 
//                 byte[] packageBytes = File.ReadAllBytes(packagePath);
//                 
//                 var package = UIPackage.AddPackage(packageBytes, assetPath,
//                     (string name, string extension, System.Type type, PackageItem packageItem) =>
//                     {
//                         Main.InvokeCoroutine(LoadAsset(modUIPath,name, extension, type, packageItem,callback));
//                     });
//                 packages[assetPath] = package;
//                 callback?.Invoke();
//             }
//             else
//             {
//                 callback?.Invoke();
//             }
//         }
//
//         public static ScriptWindow LoadScriptWindow(string scriptName)
//         {
//             foreach (var modConfig in ModManager.modConfigs)
//             {
//                 string curPath = Path.Combine(modConfig.Path, $"UIScripts/{scriptName}.cs");
//                 Main.LogInfo(curPath);
//                 if (File.Exists(curPath))
//                 {
//                     ScriptWindow window = new ScriptWindow();
//                     window.evaluator = new ExpressionEvaluator();
//                     window.bindScript = File.ReadAllText(curPath);
//                     return window;
//                 }
//             }
//             return null;
//         }
//
//         public static void CreateUI(string packageName, string component,Action<GComponent> callback)
//         {
//             AddPackage(packageName, () =>
//             {
//                 var com = UIPackage.CreateObject(packageName, component).asCom;
//                 callback.Invoke(com);
//             });
//
//         }
//
//         public static void CreateWindow(string packageName, string component,string scriptName,
//             Action<ScriptWindow> callback)
//         {
//             var key = $"{packageName}_{component}_{scriptName}";
//             if (windows.TryGetValue(key, out var window))
//             {
//                 callback.Invoke(window);
//             }
//             else
//             {
//                 window = LoadScriptWindow(scriptName);
//                 windows.Add(key,window);
//                 CreateUI(packageName, component, view =>
//                 {
//                     window.contentPane = view;
//                     callback.Invoke(window);
//                 });
//             }
//         }
//
//         public static void DestroyWindow(ScriptWindow window)
//         {
//             bool removeFlag = false;
//             string key = null;
//             foreach (var kvp in windows)
//             {
//                 if (kvp.Value == window)
//                 {
//                     removeFlag = true;
//                     key = kvp.Key;
//                     break;
//                 }
//             }
//
//             if (removeFlag)
//             {
//                 windows.Remove(key);
//                 window.Dispose();
//             }
//         }
//         
//         public static void CreateStageUI(string packageName, string component,Action<GComponent> callback)
//         {
//             CreateUI(packageName, component, view =>
//             {
//                 GRoot.inst.AddChild(view);
//             });
//         }
//
//         private static IEnumerator LoadAsset(string modUIPath,string name, string extension, System.Type type, 
//             PackageItem packageItem,Action callback)
//         {
//             var tagPath = Path.Combine(modUIPath, $"{name}{extension}");
//             if (type == typeof(TextAsset))
//             {
//                 using (var webRequest = UnityWebRequest.Get(tagPath))
//                 {
//                     yield return webRequest.SendWebRequest();
//                     if(!webRequest.isDone) {
//                         Main.LogError(webRequest.error);
//                         packageItem.owner.SetItemAsset(packageItem,null,DestroyMethod.None);
//                         yield break;
//                     }
//
//                     byte[] res = webRequest.downloadHandler.data;
//                     packageItem.owner.SetItemAsset(packageItem,res,DestroyMethod.Destroy);
//                 }
//                 
//             }
//             else if (type == typeof(AudioClip))
//             {
//                 using (var webRequest = UnityWebRequestMultimedia.GetAudioClip(tagPath, AudioType.UNKNOWN))
//                 {
//                     yield return webRequest.SendWebRequest();
//                     if(!webRequest.isDone) {
//                         Main.LogError(webRequest.error);
//                         packageItem.owner.SetItemAsset(packageItem,null,DestroyMethod.None);
//                         yield break;
//                     }
//
//                     AudioClip audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
//                     packageItem.owner.SetItemAsset(packageItem,audioClip,DestroyMethod.Destroy);
//                 }
//             }
//             else if (type == typeof(Texture) || type == typeof(Texture2D))
//             {
//                 using (var webRequest = UnityWebRequestTexture.GetTexture(tagPath))
//                 {
//                     var texDl = new DownloadHandlerTexture(true);
//                     webRequest.downloadHandler = texDl;
//                     yield return webRequest.SendWebRequest();
//                     if(!webRequest.isDone) {
//                         Main.LogError(webRequest.error);
//                         packageItem.owner.SetItemAsset(packageItem,null,DestroyMethod.None);
//                         yield break;
//                     }
//                     Texture texture = texDl.texture;
//                     packageItem.owner.SetItemAsset(packageItem,texDl.texture,DestroyMethod.Destroy);
//                 }
//             }
//             else
//             {
//                 Main.LogError($"不识别的类型：{type}");
//                 yield break;
//             }
//         }
//     }
// }