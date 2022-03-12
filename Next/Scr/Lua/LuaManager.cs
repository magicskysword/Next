using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XLua;

namespace SkySwordKill.Next.Lua
{
    public class LuaManager
    {
        public Dictionary<string, byte[]> LuaChunkCaches = new Dictionary<string, byte[]>();

        public static void Init()
        {
            Main.LogInfo($"开始加载xlua.dll");
            DllTools.LoadDllFile(Main.pathLibraryDir.Value, "xlua.dll" );
            Main.LogInfo($"加载完毕");
        }

        public LuaEnv LuaEnv;

        public LuaManager()
        {
            InitLuaEnv();
        }

        public object[] DoString(string str)
        {
            return LuaEnv.DoString(str);
        }

        public void RunFunc(string scr, string funcName, object[] args)
        {
            var rets = LuaEnv.DoString($"return require '{scr}'");
            if (rets.Length > 0 && rets[0] is LuaTable table)
            {
                var func = table.Get<LuaFunction>(funcName);
                func.Call(args);
            }
            else
            {
                Main.LogError($"读取Lua {scr} 失败");
            }
        }

        public void Clear()
        {
            InitLuaEnv();
            LuaChunkCaches.Clear();
        }
        
        private void InitLuaEnv()
        {
            if (LuaEnv != null)
            {
                LuaEnv.Dispose();
            }
            LuaEnv = new LuaEnv();
            LuaEnv.AddLoader(LuaLibLoader);
            LuaEnv.AddLoader(LuaScriptsLoader);
        }

        private byte[] LuaLibLoader(ref string filepath)
        {
            filepath = filepath.Replace(".", "/");
            filepath = $"{Main.pathLuaLibDir.Value}/{filepath}.lua";
            Main.LogDebug($"Lua载入地址 {filepath}");
            if (File.Exists(filepath))
            {
                var luaChunk = Encoding.UTF8.GetBytes(File.ReadAllText(filepath));
                return luaChunk;
            }

            return null;
        }

        private byte[] LuaScriptsLoader(ref string filepath)
        {
            filepath = filepath.Replace(".", "/");
            filepath = $"{filepath}.lua";
            Main.LogDebug($"Lua载入缓存 {filepath}");
            if (LuaChunkCaches.TryGetValue(filepath, out var luaChunk))
            {
                return luaChunk;
            }

            return null;
        }
    }
}