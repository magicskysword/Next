using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using KBEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.Next.Mod
{
    public class MainDataContainer
    {
        public Dictionary<string, JObject> dataJObjects = new Dictionary<string, JObject>();
        public Dictionary<string, JSONObject> dataJSONObjects = new Dictionary<string, JSONObject>();

        public Dictionary<string, jsonData.YSDictionary<string, JSONObject>> dataYSDics =
            new Dictionary<string, jsonData.YSDictionary<string, JSONObject>>();

        public Dictionary<string, JSONObject[]> dataJSONObjectArrays = new Dictionary<string, JSONObject[]>();

        public static MainDataContainer CloneMainData()
        {
            var dataContainer = new MainDataContainer();

            jsonData jsonInstance = jsonData.instance;
            foreach (var fieldInfo in ModManager.jsonDataFields)
            {
                if (ModManager.IsBanField(fieldInfo))
                    continue;

                var value = fieldInfo.GetValue(jsonInstance);

                if (value is JSONObject jsonObject)
                {
                    dataContainer.dataJSONObjects.Add(fieldInfo.Name,jsonObject.Copy());
                }
                else if (value is JObject jObject)
                {
                    dataContainer.dataJObjects.Add(fieldInfo.Name,(JObject)jObject.DeepClone());
                }
                else if (value is jsonData.YSDictionary<string, JSONObject> dicData)
                {
                    var ysDic = new jsonData.YSDictionary<string, JSONObject>();
                    dataContainer.dataYSDics.Add(fieldInfo.Name,ysDic);
                    foreach (var kvp in dicData)
                    {
                        ysDic.Add(kvp.Key,kvp.Value.Copy());
                    }
                }
                else if (value is JSONObject[] jsonObjects)
                {
                    var jsonArray = new JSONObject[jsonObjects.Length];
                    dataContainer.dataJSONObjectArrays.Add(fieldInfo.Name,jsonArray);
                    for (int i = 0; i < jsonObjects.Length; i++)
                    {
                        if(jsonObjects[i] == null)
                            continue;

                        jsonArray[i] = jsonObjects[i].Copy();
                    }
                }
            }
            
            return dataContainer;
        }

        public static void CoverMainData(MainDataContainer dataContainer)
        {
            jsonData jsonInstance = jsonData.instance;
            foreach (var fieldInfo in ModManager.jsonDataFields)
            {
                if (ModManager.IsBanField(fieldInfo))
                    continue;

                var value = fieldInfo.GetValue(jsonInstance);

                if (value is JSONObject)
                {
                    var jsonObject = dataContainer.dataJSONObjects[fieldInfo.Name];
                    
                    fieldInfo.SetValue(jsonInstance,jsonObject.Copy());
                }
                else if (value is JObject)
                {
                    var jObject = dataContainer.dataJObjects[fieldInfo.Name];
                    
                    fieldInfo.SetValue(jsonInstance,jObject.DeepClone());
                }
                else if (value is jsonData.YSDictionary<string, JSONObject>)
                {
                    var dicData = dataContainer.dataYSDics[fieldInfo.Name];
                    
                    var ysDic = new jsonData.YSDictionary<string, JSONObject>();
                    fieldInfo.SetValue(jsonInstance,ysDic);
                    foreach (var kvp in dicData)
                    {
                        ysDic.Add(kvp.Key,kvp.Value.Copy());
                    }
                }
                else if (value is JSONObject[])
                {
                    var jsonObjects = dataContainer.dataJSONObjectArrays[fieldInfo.Name];
                    
                    var jsonArray = new JSONObject[jsonObjects.Length];
                    fieldInfo.SetValue(jsonInstance,jsonArray);
                    for (int i = 0; i < jsonObjects.Length; i++)
                    {
                        if(jsonObjects[i] == null)
                            continue;

                        jsonArray[i] = jsonObjects[i].Copy();
                    }
                }
            }
            
            ResetYSDictionary(jsonInstance._ItemJsonData, jsonInstance.ItemJsonData);
            ResetYSDictionary(jsonInstance._BuffJsonData, jsonInstance.BuffJsonData);
            ResetYSDictionary(jsonInstance._skillJsonData, jsonInstance.skillJsonData);
            ResetYSDictionary(jsonInstance._firstNameJsonData, jsonInstance.firstNameJsonData);
            ResetYSDictionary(jsonInstance._LastNameJsonData, jsonInstance.LastNameJsonData);
            ResetYSDictionary(jsonInstance._LastWomenNameJsonData, jsonInstance.LastWomenNameJsonData);
            
            jsonInstance.Buff.Clear();
            foreach (var jsonObject in jsonInstance.BuffJsonData.Values)
            {
                var key = jsonObject["buffid"].I;
                jsonInstance.Buff.Add(key, new Buff(key));
            }
        }

        public static void ExportMainData(MainDataContainer dataContainer, string exportDir)
        {
            string dirPath = exportDir;
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
            Directory.CreateDirectory(dirPath);

            foreach (var pair in dataContainer.dataJObjects)
            {
                string filePath = Utility.CombinePaths(dirPath, $"{pair.Key}.json");
                File.WriteAllText(filePath, pair.Value.ToString(Formatting.Indented));
            }
            
            foreach (var pair in dataContainer.dataJSONObjects)
            {
                string filePath = Utility.CombinePaths(dirPath, $"{pair.Key}.json");
                File.WriteAllText(filePath, pair.Value.Print(true).DecodeJsonUnicode());
            }
            
            foreach (var pair in dataContainer.dataYSDics)
            {
                string dirPathForData = Utility.CombinePaths(dirPath, pair.Key);
                if (!Directory.Exists(dirPathForData))
                    Directory.CreateDirectory(dirPathForData);
                foreach (var kvp in pair.Value)
                {
                    string filePath = Utility.CombinePaths(dirPathForData, $"{kvp.Key}.json");
                    File.WriteAllText(filePath, kvp.Value.Print(true).DecodeJsonUnicode());
                }
            }
            
            foreach (var pair in dataContainer.dataJSONObjectArrays)
            {
                string dirPathForData = Utility.CombinePaths(dirPath, pair.Key);
                if (!Directory.Exists(dirPathForData))
                    Directory.CreateDirectory(dirPathForData);
                var jsonObjects = pair.Value;
                for (int i = 0; i < jsonObjects.Length; i++)
                {
                    if (jsonObjects[i] == null)
                        continue;
                    string filePath = Utility.CombinePaths(dirPathForData, $"{i}.json");
                    File.WriteAllText(filePath, jsonObjects[i].Print(true).DecodeJsonUnicode());
                }
            }
        }

        private static void ResetYSDictionary(JSONObject json, jsonData.YSDictionary<string, JSONObject> dict)
        {
            json.Clear();
            
            foreach (var pair in dict.OrderBy(pair => pair.Key))
            {
                json[pair.Key] = dict[pair.Key];
            }
        }
    }
}