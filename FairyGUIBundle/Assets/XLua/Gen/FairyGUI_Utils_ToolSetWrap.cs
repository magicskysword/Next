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
    public class FairyGUIUtilsToolSetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.Utils.ToolSet);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 9, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ConvertFromHtmlColor", _m_ConvertFromHtmlColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ColorFromRGB", _m_ColorFromRGB_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ColorFromRGBA", _m_ColorFromRGBA_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CharToHex", _m_CharToHex_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Intersection", _m_Intersection_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Union", _m_Union_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SkewMatrix", _m_SkewMatrix_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RotateUV", _m_RotateUV_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "FairyGUI.Utils.ToolSet does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConvertFromHtmlColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _str = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.ToolSet.ConvertFromHtmlColor( _str );
                        translator.PushUnityEngineColor(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ColorFromRGB_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _value = LuaAPI.xlua_tointeger(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.ToolSet.ColorFromRGB( _value );
                        translator.PushUnityEngineColor(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ColorFromRGBA_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    uint _value = LuaAPI.xlua_touint(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.ToolSet.ColorFromRGBA( _value );
                        translator.PushUnityEngineColor(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CharToHex_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    char _c = (char)LuaAPI.xlua_tointeger(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.ToolSet.CharToHex( _c );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Intersection_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Rect _rect1;translator.Get(L, 1, out _rect1);
                    UnityEngine.Rect _rect2;translator.Get(L, 2, out _rect2);
                    
                        var gen_ret = FairyGUI.Utils.ToolSet.Intersection( ref _rect1, ref _rect2 );
                        translator.Push(L, gen_ret);
                    translator.Push(L, _rect1);
                        translator.Update(L, 1, _rect1);
                        
                    translator.Push(L, _rect2);
                        translator.Update(L, 2, _rect2);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Union_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Rect _rect1;translator.Get(L, 1, out _rect1);
                    UnityEngine.Rect _rect2;translator.Get(L, 2, out _rect2);
                    
                        var gen_ret = FairyGUI.Utils.ToolSet.Union( ref _rect1, ref _rect2 );
                        translator.Push(L, gen_ret);
                    translator.Push(L, _rect1);
                        translator.Update(L, 1, _rect1);
                        
                    translator.Push(L, _rect2);
                        translator.Update(L, 2, _rect2);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SkewMatrix_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Matrix4x4 _matrix;translator.Get(L, 1, out _matrix);
                    float _skewX = (float)LuaAPI.lua_tonumber(L, 2);
                    float _skewY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    FairyGUI.Utils.ToolSet.SkewMatrix( ref _matrix, _skewX, _skewY );
                    translator.Push(L, _matrix);
                        translator.Update(L, 1, _matrix);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RotateUV_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector2[] _uv = (UnityEngine.Vector2[])translator.GetObject(L, 1, typeof(UnityEngine.Vector2[]));
                    UnityEngine.Rect _baseUVRect;translator.Get(L, 2, out _baseUVRect);
                    
                    FairyGUI.Utils.ToolSet.RotateUV( _uv, ref _baseUVRect );
                    translator.Push(L, _baseUVRect);
                        translator.Update(L, 2, _baseUVRect);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
