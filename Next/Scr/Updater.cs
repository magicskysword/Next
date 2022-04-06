using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Extension;

namespace SkySwordKill.Next
{
    public static class Updater
    {
        public class VersionJson
        {
            public string Version { get; set; }
        }

        public static string VersionJsonUrl => "https://raw.githubusercontent.com/magicskysword/Next/main/version.json";
        public static string WebGitHubUrl => "https://github.com/magicskysword/Next/releases/latest";
        public static string Web3dmBBSUrl => "https://bbs.3dmgame.com/thread-6207429-1-1.html";
        public static string Web3dmModSiteUrl => "https://mod.3dmgame.com/mod/178805";
        public static bool CheckSuccess { get;private set; } = false;
        public static bool HasNewVersion { get;private set; }
        public static string CurVersionStr => Main.MOD_VERSION;
        public static string NewVersionStr { get;private set; } = "0.0.0";
        public static bool IsChecking { get; set; } = false;
        
        public static async void CheckVersion()
        {
            if(IsChecking)
                return;
            
            CheckSuccess = false;
            IsChecking = true;
            WebClient wc = new WebClient();
            var result = await wc.DownloadStringTaskAsync(VersionJsonUrl);
            try
            {
                var data = JObject.Parse(result).ToObject<VersionJson>();
                if (data == null)
                    throw new Exception("Updater.JsonParseFailure".I18N());
                CompareVersion(data.Version);
                CheckSuccess = true;
            }
            catch (Exception e)
            {
                Main.LogError(new Exception("Updater.GetVersionJsonFailure".I18N(),e));
                CheckSuccess = false;
            }
            IsChecking = false;
        }

        private static void CompareVersion(string getVersionStr)
        {
            var curVersionData = new Version(CurVersionStr);
            var getVersionData = new Version(getVersionStr);

            if (curVersionData >= getVersionData)
            {
                HasNewVersion = false;
                NewVersionStr = CurVersionStr;
            }
            else
            {
                HasNewVersion = true;
                NewVersionStr = getVersionStr;
            }
        }
    }
}