using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.Next.Mod
{
    public class ModConfig
    {
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonIgnore]
        public ModState State { get; set; }
        [JsonIgnore]
        public string Path { get; set; }
        [JsonIgnore]
        public Exception Exception { get; set; }
        /// <summary>
        /// 数据版本
        /// V1版本是老版本
        /// V2版本是新版本
        /// </summary>
        [JsonIgnore]
        public int DataVersion { get; set; } = 1;

        [JsonIgnore]
        public Dictionary<string, string> jsonPathCache = new Dictionary<string, string>();

        public string GetModStateDescription()
        {
            string modState = string.Empty;
            string colorCode = string.Empty;
            
            switch (State)
            {
                case ModState.Unload:
                    modState = "Mod.Load.Unload".I18N();
                    colorCode = "#000000";
                    break;
                case ModState.Disable:
                    modState = "Mod.Load.Disable".I18N();
                    colorCode = "#808080";
                    break;
                case ModState.Loading:
                    modState = "Mod.Load.Loading".I18N();
                    colorCode = "#000000";
                    break;
                case ModState.LoadSuccess:
                    modState = "Mod.Load.Success".I18N();
                    colorCode = "#00FFFF";
                    break;
                case ModState.LoadFail:
                    modState = "Mod.Load.Fail".I18N();
                    colorCode = "#FF0000";
                    break;
            }

            return $"<color={colorCode}>{modState}</color>";
        }

        /// <summary>
        /// 游戏数据文件夹
        /// </summary>
        /// <returns></returns>
        public string GetDataDir()
        {
            if (DataVersion == 2)
            {
                return $"{Path}/Data";
            }

            return Path;
        }
        
        /// <summary>
        /// Next数据文件夹
        /// </summary>
        /// <returns></returns>
        public string GetNDataDir()
        {
            if (DataVersion == 2)
            {
                return $"{Path}/NData";
            }

            return Path;
        }

        /// <summary>
        /// 配置文件夹
        /// </summary>
        /// <returns></returns>
        public string GetConfigDir()
        {
            if (DataVersion == 2)
            {
                return $"{Path}/Config";
            }

            return Path;
        }
    }
}