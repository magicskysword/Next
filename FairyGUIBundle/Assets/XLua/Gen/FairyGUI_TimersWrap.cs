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
    public class FairyGUITimersWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.Timers);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Add", _m_Add);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallLater", _m_CallLater);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUpdate", _m_AddUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartCoroutine", _m_StartCoroutine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Exists", _m_Exists);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Remove", _m_Remove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 4, 3);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "inst", _g_get_inst);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "repeat", _g_get_repeat);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "time", _g_get_time);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "catchCallbackExceptions", _g_get_catchCallbackExceptions);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "repeat", _s_set_repeat);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "time", _s_set_time);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "catchCallbackExceptions", _s_set_catchCallbackExceptions);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.Timers();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Timers constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Add(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<FairyGUI.TimerCallback>(L, 4)) 
                {
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _repeat = LuaAPI.xlua_tointeger(L, 3);
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 4);
                    
                    gen_to_be_invoked.Add( _interval, _repeat, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<FairyGUI.TimerCallback>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    float _interval = (float)LuaAPI.lua_tonumber(L, 2);
                    int _repeat = LuaAPI.xlua_tointeger(L, 3);
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 4);
                    object _callbackParam = translator.GetObject(L, 5, typeof(object));
                    
                    gen_to_be_invoked.Add( _interval, _repeat, _callback, _callbackParam );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Timers.Add!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLater(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<FairyGUI.TimerCallback>(L, 2)) 
                {
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 2);
                    
                    gen_to_be_invoked.CallLater( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<FairyGUI.TimerCallback>(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 2);
                    object _callbackParam = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.CallLater( _callback, _callbackParam );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Timers.CallLater!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<FairyGUI.TimerCallback>(L, 2)) 
                {
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 2);
                    
                    gen_to_be_invoked.AddUpdate( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<FairyGUI.TimerCallback>(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 2);
                    object _callbackParam = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.AddUpdate( _callback, _callbackParam );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Timers.AddUpdate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartCoroutine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.IEnumerator _routine = (System.Collections.IEnumerator)translator.GetObject(L, 2, typeof(System.Collections.IEnumerator));
                    
                    gen_to_be_invoked.StartCoroutine( _routine );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Exists(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Exists( _callback );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Remove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.TimerCallback _callback = translator.GetDelegate<FairyGUI.TimerCallback>(L, 2);
                    
                    gen_to_be_invoked.Remove( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Timers gen_to_be_invoked = (FairyGUI.Timers)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_inst(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, FairyGUI.Timers.inst);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_repeat(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.Timers.repeat);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_time(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, FairyGUI.Timers.time);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_catchCallbackExceptions(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, FairyGUI.Timers.catchCallbackExceptions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_repeat(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.Timers.repeat = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_time(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.Timers.time = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_catchCallbackExceptions(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.Timers.catchCallbackExceptions = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
