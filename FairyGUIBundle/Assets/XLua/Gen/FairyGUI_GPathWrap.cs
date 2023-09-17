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
    public class FairyGUIGPathWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.GPath);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Create", _m_Create);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPointAt", _m_GetPointAt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSegmentLength", _m_GetSegmentLength);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPointsInSegment", _m_GetPointsInSegment);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllPoints", _m_GetAllPoints);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "length", _g_get_length);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "segmentCount", _g_get_segmentCount);
            
			
			
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
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.GPath();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GPath constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.IEnumerable<FairyGUI.GPathPoint>>(L, 2)) 
                {
                    System.Collections.Generic.IEnumerable<FairyGUI.GPathPoint> _points = (System.Collections.Generic.IEnumerable<FairyGUI.GPathPoint>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IEnumerable<FairyGUI.GPathPoint>));
                    
                    gen_to_be_invoked.Create( _points );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<FairyGUI.GPathPoint>(L, 2)&& translator.Assignable<FairyGUI.GPathPoint>(L, 3)) 
                {
                    FairyGUI.GPathPoint _pt1;translator.Get(L, 2, out _pt1);
                    FairyGUI.GPathPoint _pt2;translator.Get(L, 3, out _pt2);
                    
                    gen_to_be_invoked.Create( _pt1, _pt2 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<FairyGUI.GPathPoint>(L, 2)&& translator.Assignable<FairyGUI.GPathPoint>(L, 3)&& translator.Assignable<FairyGUI.GPathPoint>(L, 4)) 
                {
                    FairyGUI.GPathPoint _pt1;translator.Get(L, 2, out _pt1);
                    FairyGUI.GPathPoint _pt2;translator.Get(L, 3, out _pt2);
                    FairyGUI.GPathPoint _pt3;translator.Get(L, 4, out _pt3);
                    
                    gen_to_be_invoked.Create( _pt1, _pt2, _pt3 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<FairyGUI.GPathPoint>(L, 2)&& translator.Assignable<FairyGUI.GPathPoint>(L, 3)&& translator.Assignable<FairyGUI.GPathPoint>(L, 4)&& translator.Assignable<FairyGUI.GPathPoint>(L, 5)) 
                {
                    FairyGUI.GPathPoint _pt1;translator.Get(L, 2, out _pt1);
                    FairyGUI.GPathPoint _pt2;translator.Get(L, 3, out _pt2);
                    FairyGUI.GPathPoint _pt3;translator.Get(L, 4, out _pt3);
                    FairyGUI.GPathPoint _pt4;translator.Get(L, 5, out _pt4);
                    
                    gen_to_be_invoked.Create( _pt1, _pt2, _pt3, _pt4 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GPath.Create!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPointAt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _t = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPointAt( _t );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSegmentLength(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _segmentIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetSegmentLength( _segmentIndex );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPointsInSegment(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Vector3>>(L, 5)&& translator.Assignable<System.Collections.Generic.List<float>>(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    int _segmentIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _t0 = (float)LuaAPI.lua_tonumber(L, 3);
                    float _t1 = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Collections.Generic.List<UnityEngine.Vector3> _points = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 5, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
                    System.Collections.Generic.List<float> _ts = (System.Collections.Generic.List<float>)translator.GetObject(L, 6, typeof(System.Collections.Generic.List<float>));
                    float _pointDensity = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.GetPointsInSegment( _segmentIndex, _t0, _t1, _points, _ts, _pointDensity );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Vector3>>(L, 5)&& translator.Assignable<System.Collections.Generic.List<float>>(L, 6)) 
                {
                    int _segmentIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _t0 = (float)LuaAPI.lua_tonumber(L, 3);
                    float _t1 = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Collections.Generic.List<UnityEngine.Vector3> _points = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 5, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
                    System.Collections.Generic.List<float> _ts = (System.Collections.Generic.List<float>)translator.GetObject(L, 6, typeof(System.Collections.Generic.List<float>));
                    
                    gen_to_be_invoked.GetPointsInSegment( _segmentIndex, _t0, _t1, _points, _ts );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Vector3>>(L, 5)) 
                {
                    int _segmentIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _t0 = (float)LuaAPI.lua_tonumber(L, 3);
                    float _t1 = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Collections.Generic.List<UnityEngine.Vector3> _points = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 5, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
                    
                    gen_to_be_invoked.GetPointsInSegment( _segmentIndex, _t0, _t1, _points );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GPath.GetPointsInSegment!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllPoints(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Vector3>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    System.Collections.Generic.List<UnityEngine.Vector3> _points = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
                    float _pointDensity = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.GetAllPoints( _points, _pointDensity );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<UnityEngine.Vector3>>(L, 2)) 
                {
                    System.Collections.Generic.List<UnityEngine.Vector3> _points = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
                    
                    gen_to_be_invoked.GetAllPoints( _points );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GPath.GetAllPoints!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_length(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.length);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_segmentCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GPath gen_to_be_invoked = (FairyGUI.GPath)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.segmentCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
