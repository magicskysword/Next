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
    public class FairyGUIUtilsHtmlPageContextWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.Utils.HtmlPageContext);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateObject", _m_CreateObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FreeObject", _m_FreeObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetImageTexture", _m_GetImageTexture);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FreeImageTexture", _m_FreeImageTexture);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "inst", _g_get_inst);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "inst", _s_set_inst);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.Utils.HtmlPageContext();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.HtmlPageContext constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Utils.HtmlPageContext gen_to_be_invoked = (FairyGUI.Utils.HtmlPageContext)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.RichTextField _owner = (FairyGUI.RichTextField)translator.GetObject(L, 2, typeof(FairyGUI.RichTextField));
                    FairyGUI.Utils.HtmlElement _element = (FairyGUI.Utils.HtmlElement)translator.GetObject(L, 3, typeof(FairyGUI.Utils.HtmlElement));
                    
                        var gen_ret = gen_to_be_invoked.CreateObject( _owner, _element );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FreeObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Utils.HtmlPageContext gen_to_be_invoked = (FairyGUI.Utils.HtmlPageContext)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.Utils.IHtmlObject _obj = (FairyGUI.Utils.IHtmlObject)translator.GetObject(L, 2, typeof(FairyGUI.Utils.IHtmlObject));
                    
                    gen_to_be_invoked.FreeObject( _obj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetImageTexture(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Utils.HtmlPageContext gen_to_be_invoked = (FairyGUI.Utils.HtmlPageContext)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.Utils.HtmlImage _image = (FairyGUI.Utils.HtmlImage)translator.GetObject(L, 2, typeof(FairyGUI.Utils.HtmlImage));
                    
                        var gen_ret = gen_to_be_invoked.GetImageTexture( _image );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FreeImageTexture(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.Utils.HtmlPageContext gen_to_be_invoked = (FairyGUI.Utils.HtmlPageContext)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.Utils.HtmlImage _image = (FairyGUI.Utils.HtmlImage)translator.GetObject(L, 2, typeof(FairyGUI.Utils.HtmlImage));
                    FairyGUI.NTexture _texture = (FairyGUI.NTexture)translator.GetObject(L, 3, typeof(FairyGUI.NTexture));
                    
                    gen_to_be_invoked.FreeImageTexture( _image, _texture );
                    
                    
                    
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
			    translator.Push(L, FairyGUI.Utils.HtmlPageContext.inst);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_inst(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    FairyGUI.Utils.HtmlPageContext.inst = (FairyGUI.Utils.HtmlPageContext)translator.GetObject(L, 1, typeof(FairyGUI.Utils.HtmlPageContext));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
