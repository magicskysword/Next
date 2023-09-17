#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class FairyGUIBlendModeUtilsBlendFactorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.BlendModeUtils.BlendFactor);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 3, 3);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "srcFactor", _g_get_srcFactor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "dstFactor", _g_get_dstFactor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pma", _g_get_pma);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "srcFactor", _s_set_srcFactor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "dstFactor", _s_set_dstFactor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pma", _s_set_pma);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 4 && translator.Assignable<UnityEngine.Rendering.BlendMode>(L, 2) && translator.Assignable<UnityEngine.Rendering.BlendMode>(L, 3) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4))
				{
					UnityEngine.Rendering.BlendMode _srcFactor;translator.Get(L, 2, out _srcFactor);
					UnityEngine.Rendering.BlendMode _dstFactor;translator.Get(L, 3, out _dstFactor);
					bool _pma = LuaAPI.lua_toboolean(L, 4);
					
					var gen_ret = new FairyGUI.BlendModeUtils.BlendFactor(_srcFactor, _dstFactor, _pma);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<UnityEngine.Rendering.BlendMode>(L, 2) && translator.Assignable<UnityEngine.Rendering.BlendMode>(L, 3))
				{
					UnityEngine.Rendering.BlendMode _srcFactor;translator.Get(L, 2, out _srcFactor);
					UnityEngine.Rendering.BlendMode _dstFactor;translator.Get(L, 3, out _dstFactor);
					
					var gen_ret = new FairyGUI.BlendModeUtils.BlendFactor(_srcFactor, _dstFactor);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.BlendModeUtils.BlendFactor constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_srcFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.BlendModeUtils.BlendFactor gen_to_be_invoked = (FairyGUI.BlendModeUtils.BlendFactor)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.srcFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dstFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.BlendModeUtils.BlendFactor gen_to_be_invoked = (FairyGUI.BlendModeUtils.BlendFactor)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.dstFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pma(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.BlendModeUtils.BlendFactor gen_to_be_invoked = (FairyGUI.BlendModeUtils.BlendFactor)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.pma);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_srcFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.BlendModeUtils.BlendFactor gen_to_be_invoked = (FairyGUI.BlendModeUtils.BlendFactor)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.BlendMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.srcFactor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_dstFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.BlendModeUtils.BlendFactor gen_to_be_invoked = (FairyGUI.BlendModeUtils.BlendFactor)translator.FastGetCSObj(L, 1);
                UnityEngine.Rendering.BlendMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.dstFactor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pma(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.BlendModeUtils.BlendFactor gen_to_be_invoked = (FairyGUI.BlendModeUtils.BlendFactor)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pma = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
