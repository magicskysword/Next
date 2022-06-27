using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XLua;

namespace SkySwordKill.Next.Lua
{
    public class LuaManager
    {
        public Dictionary<string, LuaFileCache> LuaCaches = new Dictionary<string, LuaFileCache>();

        public void Init()
        {
            Main.LogInfo($"开始加载xlua.dll");
            DllTools.LoadDllFile(Main.PathLibraryDir.Value, "xlua.dll" );
            Main.LogInfo($"加载完毕");
            InitLuaEnv();
        }

        public LuaEnv LuaEnv;

        public object[] DoString(string str)
        {
            return LuaEnv.DoString(str);
        }

        public object[] RunFunc(string scr, string funcName, object[] args)
        {
            var rets = LuaEnv.DoString($"return require '{scr}'");
            if (rets.Length > 0 && rets[0] is LuaTable table)
            {
                var func = table.Get<LuaFunction>(funcName);
                return func.Call(args);
            }
            else
            {
                Main.LogError($"读取Lua {scr} 失败");
                return null;
            }
        }

        public void Reset()
        {
            InitLuaEnv();
            LuaCaches.Clear();
        }
        
        private void InitLuaEnv()
        {
            GC.Collect();
            if (LuaEnv != null)
            {
                LuaEnv.DoString(@"
collectgarbage(""collect"")
");
                /*LuaEnv.DoString(@"
local util = require 'xlua.util'
print(""Lua callback ref trace:"")
util.print_func_ref_by_csharp()");*/
                try
                {
                    LuaEnv.Dispose();
                }
                catch (Exception e)
                {
                    Main.LogWarning(e);
                }
            }
            LuaEnv = new LuaEnv();
            LuaEnv.AddLoader(LuaLibLoader);
            LuaEnv.AddLoader(LuaScriptsLoader);
            LuaEnv.DoString(@"
function print(...)
    local msgtb = {}
    for key, value in pairs({...}) do
        table.insert(msgtb,tostring(value))
    end
    local msg = table.concat(msgtb,""\t"")
    CS.SkySwordKill.Next.Main.LogLua(msg)
end
function getmodpath(luafile)
    return CS.SkySwordKill.Next.Main.Instance.luaManager:GetLuaModPath(luafile)
end
");
        }

        private byte[] LuaLibLoader(ref string filepath)
        {
            filepath = filepath.Replace(@".", @"/");
            filepath = $"{Main.PathLuaLibDir.Value}/{filepath}.lua"
                .Replace(@"\", @"/");
            if (File.Exists(filepath))
            {
                Main.LogDebug($"Lua载入地址 {filepath}");
                var luaChunk = Encoding.UTF8.GetBytes(File.ReadAllText(filepath));
                return luaChunk;
            }

            return null;
        }

        private byte[] LuaScriptsLoader(ref string filepath)
        {
            var virtualPath = filepath.Replace(".", "/");
            if (LuaCaches.TryGetValue(virtualPath, out var luaCache))
            {
                filepath = luaCache.FilePath;
                if (File.Exists(filepath))
                {
                    Main.LogDebug($"Lua载入缓存 [{luaCache.FromMod.Path}] {filepath}");
                    var luaChunk = Encoding.UTF8.GetBytes(File.ReadAllText(filepath));
                    return luaChunk;
                }
            }
            return null;
        }

        public void AddLuaCacheFile(string luaPath,LuaFileCache luaCache)
        {
            if (LuaCaches.ContainsKey(luaPath))
            {
                var oldLuaCache = LuaCaches[luaPath];
                Main.LogWarning($"Lua\"{luaPath}\"发生覆盖 [{oldLuaCache.FromMod.Name}]{oldLuaCache.FilePath} --> " +
                                $"[{luaCache.FromMod.Name}]{luaCache.FilePath}");
            }
            else
            {
                Main.LogInfo($"添加Lua指向：{luaPath}.lua");
            }

            LuaCaches[luaPath] = luaCache;
        }

        public string GetLuaModPath(string filepath)
        {
            var virtualPath = filepath.Replace(".", "/");
            if (LuaCaches.TryGetValue(virtualPath, out var luaCache))
            {
                return luaCache.FromMod.Path;
            }

            return null;
        }

        public void ClearCache()
        {
            DoString(@"
for key, _ in pairs(package.preload) do
    package.preload[key] = nil
end
for key, _ in pairs(package.loaded) do
    package.loaded[key] = nil
end
");
            
        }
    }
}