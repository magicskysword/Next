using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SkySwordKill.Next;
using SkySwordKill.NextModEditor.Mod.Data;
using UnityEngine;

namespace SkySwordKill.NextEditor.Mod
{
    public static class ModUtils
    {
        public static void ModSort<T>(this List<T> list) where T : IModData
        {
            list.Sort((dataX, dataY) => dataX.ID.CompareTo(dataY.ID));
        }

        public static int TryFind<T>(this List<T> list, Predicate<T> predicate)
        {
            var index = list.FindIndex(predicate);
            if (index < 0)
                index = 0;
            return index;
        }

        public static bool TryFormatToListInt(this string str, out List<int> list)
        {
            list = new List<int>();

            foreach (var getStr in str.Split(',', '，'))
            {
                if (string.IsNullOrWhiteSpace(getStr))
                    continue;
                if (int.TryParse(getStr, out var num))
                {
                    list.Add(num);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static string ToFormatString(this List<int> list)
        {
            return string.Join(",", list);
        }

        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public static string LoadConfig(string path)
        {
            var targetPath = GetConfigPath(path);
            if (File.Exists(targetPath))
                return File.ReadAllText(targetPath);
            Debug.LogWarning($"目标文件 {targetPath} 不存在！");
            return string.Empty;
        }

        public static string LoadBaseConfig(string path)
        {
            var targetPath = GetBasePath(path);
            if (File.Exists(targetPath))
                return File.ReadAllText(targetPath);
            Debug.LogWarning($"目标文件 {targetPath} 不存在！");
            return string.Empty;
        }
        
        
        public static string GetConfigPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return $"{Main.PathLanguageDir}/{Main.I.NextLanguage.ConfigDir}";
            return $"{Main.PathLanguageDir}/{Main.I.NextLanguage.ConfigDir}/Config/{path}";
        }
        
        public static string GetBasePath(string path = "")
        {
            if (string.IsNullOrWhiteSpace(path))
                return Main.PathBaseDataDir.Value;
            return $"{Main.PathBaseDataDir.Value}/{path}";
        }
        
        public static string GetFungusDataPath()
        {
            return Main.PathBaseFungusDataDir.Value;
        }

        public static string GetAffixDesc(ModAffixData affixData)
        {
            if (affixData == null)
            {
                return "【？】";
            }
            else
            {
                return $"【{affixData.Name}】{affixData.Desc}";
            }
        }

        public static string GetAffixDesc(this ModProject project, List<int> affixIntList)
        {
            var sb = new StringBuilder();
            foreach (var id in affixIntList)
            {
                var affixData = project.FindAffix(id);
                sb.Append(GetAffixDesc(affixData));
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public static IEnumerable<Type> GetTypesWithAttribute(Assembly assembly, Type attributeType)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(attributeType, true).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}