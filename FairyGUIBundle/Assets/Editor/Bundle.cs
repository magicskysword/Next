using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles : MonoBehaviour
{
    [MenuItem("Tools/打包AB包")] //特性
    static void BuildAssetBundle()
    {
        string dir = Application.dataPath + "/../../Next/NextLib/AB"; //相对路径
        Debug.Log($"AB包输出地址：{dir}");
        if (!Directory.Exists(dir))   //判断路径是否存在
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}