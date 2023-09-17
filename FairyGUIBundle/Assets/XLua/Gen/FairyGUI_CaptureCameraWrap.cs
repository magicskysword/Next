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
    public class FairyGUICaptureCameraWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.CaptureCamera);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 2, 2);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "cachedTransform", _g_get_cachedTransform);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cachedCamera", _g_get_cachedCamera);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "cachedTransform", _s_set_cachedTransform);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cachedCamera", _s_set_cachedCamera);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 2, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckMain", _m_CheckMain_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateRenderTexture", _m_CreateRenderTexture_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Capture", _m_Capture_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Name", FairyGUI.CaptureCamera.Name);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LayerName", FairyGUI.CaptureCamera.LayerName);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HiddenLayerName", FairyGUI.CaptureCamera.HiddenLayerName);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "layer", _g_get_layer);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "hiddenLayer", _g_get_hiddenLayer);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.CaptureCamera();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.CaptureCamera constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckMain_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    FairyGUI.CaptureCamera.CheckMain(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateRenderTexture_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _width = LuaAPI.xlua_tointeger(L, 1);
                    int _height = LuaAPI.xlua_tointeger(L, 2);
                    bool _stencilSupport = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = FairyGUI.CaptureCamera.CreateRenderTexture( _width, _height, _stencilSupport );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Capture_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    FairyGUI.DisplayObject _target = (FairyGUI.DisplayObject)translator.GetObject(L, 1, typeof(FairyGUI.DisplayObject));
                    UnityEngine.RenderTexture _texture = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
                    float _contentHeight = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector2 _offset;translator.Get(L, 4, out _offset);
                    
                    FairyGUI.CaptureCamera.Capture( _target, _texture, _contentHeight, _offset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layer(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.CaptureCamera.layer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hiddenLayer(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.CaptureCamera.hiddenLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cachedTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.CaptureCamera gen_to_be_invoked = (FairyGUI.CaptureCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cachedTransform);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cachedCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.CaptureCamera gen_to_be_invoked = (FairyGUI.CaptureCamera)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cachedCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cachedTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.CaptureCamera gen_to_be_invoked = (FairyGUI.CaptureCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cachedTransform = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cachedCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.CaptureCamera gen_to_be_invoked = (FairyGUI.CaptureCamera)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cachedCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
