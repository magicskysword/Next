using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using script.Steam;
using SkySwordKill.Next;
using SkySwordKill.Next.Utils;
using SkySwordKill.NextModEditor.Mod.Data;
using Steamworks;
using UnityEngine;

namespace SkySwordKill.NextEditor.Mod
{
    public static class ModUtils
    {
        public static List<T> ModSort<T>(this List<T> list) where T : IModData
        {
            list.Sort((dataX, dataY) => dataX.Id.CompareTo(dataY.Id));
            return list;
        }
        
        public static bool TryFindData<T>(this List<T> list, int id, out T data) where T : IModData
        {
            data = default;
            var index = list.FindIndex(x => x.Id == id);
            if (index < 0)
                return false;
            data = list[index];
            return true;
        }
        
        public static bool HasId<T>(this List<T> list, int id) where T : IModData
        {
            return list.FindIndex(data => data.Id == id) != -1;
        }
        
        public static int GetIndex<T>(this List<T> list, int id) where T : IModData
        {
            return list.FindIndex(data => data.Id == id);
        }
        
        public static JObject GetJsonData(this IModData modData)
        {
            return JObject.FromObject(modData);
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
        
        public static string GetItemFlagDesc(ModItemFlagData flagData)
        {
            if (flagData == null)
            {
                return "【？】";
            }
            else
            {
                return $"【{flagData.Id}】{flagData.Name}";
            }
        }
        
        public static string GetItemInfo(this ModWorkshop mod, ModItemData item)
        {
            if(item.ItemType == 3 || item.ItemType == 4)
            {
                return $"【技能描述】{(item.ItemType == 3 ? "神通" : "功法")}:{item.Desc}";
            }
            else
            {
                return item.Info;
            }
        }

        public static string GetAffixDesc(this ModWorkshop mod, List<int> affixIntList)
        {
            var sb = new StringBuilder();
            foreach (var id in affixIntList)
            {
                var affixData = mod.FindAffix(id);
                sb.Append(GetAffixDesc(affixData));
                sb.Append("\n");
            }

            return sb.ToString();
        }
        
        public static string GetItemFlagDesc(this ModWorkshop mod, List<int> flagList)
        {
            var sb = new StringBuilder();
            foreach (var id in flagList)
            {
                var flagData = mod.FindItemFlag(id);
                sb.Append(GetItemFlagDesc(flagData));
                sb.Append("\n");
            }

            return sb.ToString();
        }
        
        public static string GetAlchemyElementDesc(this ModWorkshop mod, int id)
        {
            if (id == 0)
                return "【0】无";
            
            var alchemyData = mod.FindAlchemyElement(id);
            if (alchemyData == null)
            {
                return $"【{id}】未知";
            }
            else
            {
                return $"【{alchemyData.Id}】{alchemyData.Name}";
            }
        }
        
        public static string GetComprehensionWithPhaseDesc(this ModWorkshop mod, List<int> comList)
        {
            var sb = new StringBuilder();
            for (var index = 0; index < comList.Count; index += 2)
            {
                var id = comList[index];
                var comData = mod.FindComprehension(id);
                
                if(comData != null)
                {
                    sb.Append($"【{id} {comData.Desc}】");
                }
                else
                {
                    sb.Append($"【{id} 未知】");
                }

                var phase = 0;
                if(index + 1 < comList.Count)
                {
                    phase = comList[index + 1];
                }
                var phaseData = mod.FindComprehensionPhase(phase);
                if(phaseData != null)
                {
                    sb.Append($"({phase}){phaseData.Name}");
                }
                else
                {
                    sb.Append($"({phase})未知");
                }
                
                if (index + 2 < comList.Count)
                {
                    sb.Append("\n");
                }
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
        
        public static WorkShopItem ReadConfig(string path)
        {
            WorkShopItem result = new WorkShopItem();
            try
            {
                FileStream fileStream = new FileStream(path + "/Mod.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                result = (WorkShopItem)new BinaryFormatter().Deserialize(fileStream);
                fileStream.Close();
            }
            catch (Exception message)
            {
                Debug.LogError(message);
                Debug.LogError("读取配置文件失败");
            }
            result.SteamID = SteamUser.GetSteamID().m_SteamID;
            result.ModPath = path;
            return result;
        }
        
        public static void WriteConfig(string path, WorkShopItem item)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            new BinaryFormatter().Serialize(fileStream, item);
            fileStream.Close();
        }

        public static string GetSkillLevelName(int itemQuality, int itemPhase)
        {
            ModEditorManager.I.SkillDataQuality.TryFindData(itemQuality, out var quality);
            ModEditorManager.I.SkillDataPhase.TryFindData(itemPhase, out var phase);
            return $"{quality?.Desc ?? "？"}{phase?.Desc ?? "？"}";
        }

        public static string AnalysisRefData(ModWorkshop mod,string str)
        {
            if (str.StartsWith("GuideLink://", StringComparison.OrdinalIgnoreCase))
            {
                var sb = new StringBuilder();
                sb.Append("【图鉴信息】");
                sb.Append("\n");
                var data = str.Substring("GuideLink://".Length);
                var split = data.Split('_');
                if (int.TryParse(split[0], out var type))
                {
                    switch (type)
                    {
                        case 1:
                            sb.Append($"类型：【{type} 实体】");
                            break;
                        case 2:
                            sb.Append($"类型：【{type} 规则】");
                            
                            sb.Append("\n");
                            if(split.Length >= 2 && int.TryParse(split[1], out var subType))
                            {
                                var affixType = ModEditorManager.I.AffixDataProjectTypes.Find(
                                    x => x.TypeNum == subType);
                                if(affixType != null)
                                    sb.Append($"标签：【{affixType.TypeNum} {affixType.TypeName}】");
                                else 
                                    sb.Append($"标签：【{subType} 未知】");
                            }
                            else
                            {
                                subType = 0;
                                sb.Append($"标签：【{subType} 未知】");
                            }
                            
                            sb.Append("\n");
                            if(split.Length >= 3 && int.TryParse(split[2], out var affixSubType))
                            {
                                var affixType = ModEditorManager.I.AffixDataAffixTypes.Find(
                                    x => x.TypeID == affixSubType);
                                if(affixType != null)
                                    sb.Append($"子类：【{affixType.TypeID} {affixType.TypeName}】");
                                else
                                    sb.Append($"子类：【{affixSubType} 未知】");
                            }
                            else
                            {
                                affixSubType = 0;
                                sb.Append($"子类：【{affixSubType} 未知】");
                            }
                    
                            sb.Append("\n");
                            if(split.Length >= 4 && int.TryParse(split[3], out var affixID))
                            {
                                var affix = mod.FindAffix(affixID);
                                if(affix != null)
                                    sb.Append($"词缀：【{affixID} {affix.Name}】{affix.Desc}");
                                else
                                    sb.Append($"词缀：【{affixID} 未知】");
                            }
                            else
                            {
                                affixID = 0;
                                sb.Append($"词缀：【{affixID} 未知】");
                            }
                            break;
                        case 3:
                            sb.Append($"类型：【{type} 地图】");
                            break;
                        default:
                            sb.Append($"类型：【{type} 未知】");
                            break;
                    }
                }
                else
                {
                    type = 0;
                    sb.Append($"类型：【{type} 未知】");
                }

                return sb.ToString();
            }
            else
            {
                return str;
            }
        }
    }
}