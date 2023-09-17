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
    public class FairyGUIShapeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.Shape);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawRect", _m_DrawRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawRoundRect", _m_DrawRoundRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawEllipse", _m_DrawEllipse);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawPolygon", _m_DrawPolygon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawRegularPolygon", _m_DrawRegularPolygon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "color", _g_get_color);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isEmpty", _g_get_isEmpty);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "color", _s_set_color);
            
			
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
					
					var gen_ret = new FairyGUI.Shape();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Shape constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Color32[]>(L, 3)) 
                {
                    float _lineSize = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Color32[] _colors = (UnityEngine.Color32[])translator.GetObject(L, 3, typeof(UnityEngine.Color32[]));
                    
                    gen_to_be_invoked.DrawRect( _lineSize, _colors );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& translator.Assignable<UnityEngine.Color>(L, 4)) 
                {
                    float _lineSize = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Color _lineColor;translator.Get(L, 3, out _lineColor);
                    UnityEngine.Color _fillColor;translator.Get(L, 4, out _fillColor);
                    
                    gen_to_be_invoked.DrawRect( _lineSize, _lineColor, _fillColor );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Shape.DrawRect!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawRoundRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _lineSize = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Color _lineColor;translator.Get(L, 3, out _lineColor);
                    UnityEngine.Color _fillColor;translator.Get(L, 4, out _fillColor);
                    float _topLeftRadius = (float)LuaAPI.lua_tonumber(L, 5);
                    float _topRightRadius = (float)LuaAPI.lua_tonumber(L, 6);
                    float _bottomLeftRadius = (float)LuaAPI.lua_tonumber(L, 7);
                    float _bottomRightRadius = (float)LuaAPI.lua_tonumber(L, 8);
                    
                    gen_to_be_invoked.DrawRoundRect( _lineSize, _lineColor, _fillColor, _topLeftRadius, _topRightRadius, _bottomLeftRadius, _bottomRightRadius );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawEllipse(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Color>(L, 2)) 
                {
                    UnityEngine.Color _fillColor;translator.Get(L, 2, out _fillColor);
                    
                    gen_to_be_invoked.DrawEllipse( _fillColor );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& translator.Assignable<UnityEngine.Color>(L, 4)&& translator.Assignable<UnityEngine.Color>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    float _lineSize = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Color _centerColor;translator.Get(L, 3, out _centerColor);
                    UnityEngine.Color _lineColor;translator.Get(L, 4, out _lineColor);
                    UnityEngine.Color _fillColor;translator.Get(L, 5, out _fillColor);
                    float _startDegree = (float)LuaAPI.lua_tonumber(L, 6);
                    float _endDegree = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.DrawEllipse( _lineSize, _centerColor, _lineColor, _fillColor, _startDegree, _endDegree );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Shape.DrawEllipse!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawPolygon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<System.Collections.Generic.IList<UnityEngine.Vector2>>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)) 
                {
                    System.Collections.Generic.IList<UnityEngine.Vector2> _points = (System.Collections.Generic.IList<UnityEngine.Vector2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IList<UnityEngine.Vector2>));
                    UnityEngine.Color _fillColor;translator.Get(L, 3, out _fillColor);
                    
                    gen_to_be_invoked.DrawPolygon( _points, _fillColor );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<System.Collections.Generic.IList<UnityEngine.Vector2>>(L, 2)&& translator.Assignable<UnityEngine.Color32[]>(L, 3)) 
                {
                    System.Collections.Generic.IList<UnityEngine.Vector2> _points = (System.Collections.Generic.IList<UnityEngine.Vector2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IList<UnityEngine.Vector2>));
                    UnityEngine.Color32[] _colors = (UnityEngine.Color32[])translator.GetObject(L, 3, typeof(UnityEngine.Color32[]));
                    
                    gen_to_be_invoked.DrawPolygon( _points, _colors );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<System.Collections.Generic.IList<UnityEngine.Vector2>>(L, 2)&& translator.Assignable<UnityEngine.Color>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Color>(L, 5)) 
                {
                    System.Collections.Generic.IList<UnityEngine.Vector2> _points = (System.Collections.Generic.IList<UnityEngine.Vector2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IList<UnityEngine.Vector2>));
                    UnityEngine.Color _fillColor;translator.Get(L, 3, out _fillColor);
                    float _lineSize = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Color _lineColor;translator.Get(L, 5, out _lineColor);
                    
                    gen_to_be_invoked.DrawPolygon( _points, _fillColor, _lineSize, _lineColor );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Shape.DrawPolygon!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawRegularPolygon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _sides = LuaAPI.xlua_tointeger(L, 2);
                    float _lineSize = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Color _centerColor;translator.Get(L, 4, out _centerColor);
                    UnityEngine.Color _lineColor;translator.Get(L, 5, out _lineColor);
                    UnityEngine.Color _fillColor;translator.Get(L, 6, out _fillColor);
                    float _rotation = (float)LuaAPI.lua_tonumber(L, 7);
                    float[] _distances = (float[])translator.GetObject(L, 8, typeof(float[]));
                    
                    gen_to_be_invoked.DrawRegularPolygon( _sides, _lineSize, _centerColor, _lineColor, _fillColor, _rotation, _distances );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_color(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.color);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isEmpty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isEmpty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_color(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.Shape gen_to_be_invoked = (FairyGUI.Shape)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.color = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
