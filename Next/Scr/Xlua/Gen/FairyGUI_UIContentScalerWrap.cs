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
    public class FairyGUIUIContentScalerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.UIContentScaler);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 8, 8);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyModifiedProperties", _m_ApplyModifiedProperties);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyChange", _m_ApplyChange);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "scaleMode", _g_get_scaleMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "screenMatchMode", _g_get_screenMatchMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "designResolutionX", _g_get_designResolutionX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "designResolutionY", _g_get_designResolutionY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fallbackScreenDPI", _g_get_fallbackScreenDPI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "defaultSpriteDPI", _g_get_defaultSpriteDPI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "constantScaleFactor", _g_get_constantScaleFactor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ignoreOrientation", _g_get_ignoreOrientation);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "scaleMode", _s_set_scaleMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "screenMatchMode", _s_set_screenMatchMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "designResolutionX", _s_set_designResolutionX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "designResolutionY", _s_set_designResolutionY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fallbackScreenDPI", _s_set_fallbackScreenDPI);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "defaultSpriteDPI", _s_set_defaultSpriteDPI);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "constantScaleFactor", _s_set_constantScaleFactor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ignoreOrientation", _s_set_ignoreOrientation);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 2, 2);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "scaleFactor", _g_get_scaleFactor);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "scaleLevel", _g_get_scaleLevel);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "scaleFactor", _s_set_scaleFactor);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "scaleLevel", _s_set_scaleLevel);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.UIContentScaler();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.UIContentScaler constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyModifiedProperties(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ApplyModifiedProperties(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyChange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ApplyChange(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scaleMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.scaleMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_screenMatchMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.screenMatchMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_designResolutionX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.designResolutionX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_designResolutionY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.designResolutionY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fallbackScreenDPI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.fallbackScreenDPI);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_defaultSpriteDPI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.defaultSpriteDPI);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_constantScaleFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.constantScaleFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ignoreOrientation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ignoreOrientation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scaleFactor(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, FairyGUI.UIContentScaler.scaleFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scaleLevel(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.UIContentScaler.scaleLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scaleMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                FairyGUI.UIContentScaler.ScaleMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.scaleMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_screenMatchMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                FairyGUI.UIContentScaler.ScreenMatchMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.screenMatchMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_designResolutionX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.designResolutionX = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_designResolutionY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.designResolutionY = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fallbackScreenDPI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.fallbackScreenDPI = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_defaultSpriteDPI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.defaultSpriteDPI = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_constantScaleFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.constantScaleFactor = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ignoreOrientation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.UIContentScaler gen_to_be_invoked = (FairyGUI.UIContentScaler)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ignoreOrientation = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scaleFactor(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.UIContentScaler.scaleFactor = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scaleLevel(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.UIContentScaler.scaleLevel = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
