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
    public class FairyGUIRTLSupportWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.RTLSupport);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 8, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "IsArabicLetter", _m_IsArabicLetter_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ConvertNumber", _m_ConvertNumber_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ContainsArabicLetters", _m_ContainsArabicLetters_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DetectTextDirection", _m_DetectTextDirection_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoMapping", _m_DoMapping_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ConvertLineL", _m_ConvertLineL_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ConvertLineR", _m_ConvertLineR_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "BaseDirection", _g_get_BaseDirection);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "BaseDirection", _s_set_BaseDirection);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.RTLSupport();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.RTLSupport constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsArabicLetter_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    char _ch = (char)LuaAPI.xlua_tointeger(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.IsArabicLetter( _ch );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConvertNumber_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _strNumber = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.ConvertNumber( _strNumber );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ContainsArabicLetters_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.ContainsArabicLetters( _text );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DetectTextDirection_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.DetectTextDirection( _text );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoMapping_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _input = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.DoMapping( _input );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConvertLineL_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _source = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.ConvertLineL( _source );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConvertLineR_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _source = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.RTLSupport.ConvertLineR( _source );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BaseDirection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, FairyGUI.RTLSupport.BaseDirection);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BaseDirection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			FairyGUI.RTLSupport.DirectionType gen_value;translator.Get(L, 1, out gen_value);
				FairyGUI.RTLSupport.BaseDirection = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
