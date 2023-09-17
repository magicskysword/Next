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
    public class FairyGUIUIObjectFactoryWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.UIObjectFactory);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetPackageItemExtension", _m_SetPackageItemExtension_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLoaderExtension", _m_SetLoaderExtension_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Clear", _m_Clear_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NewObject", _m_NewObject_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.UIObjectFactory();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.UIObjectFactory constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPackageItemExtension_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 2)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                    FairyGUI.UIObjectFactory.SetPackageItemExtension( _url, _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<FairyGUI.UIObjectFactory.GComponentCreator>(L, 2)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    FairyGUI.UIObjectFactory.GComponentCreator _creator = translator.GetDelegate<FairyGUI.UIObjectFactory.GComponentCreator>(L, 2);
                    
                    FairyGUI.UIObjectFactory.SetPackageItemExtension( _url, _creator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.UIObjectFactory.SetPackageItemExtension!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLoaderExtension_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Type>(L, 1)) 
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    
                    FairyGUI.UIObjectFactory.SetLoaderExtension( _type );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& translator.Assignable<FairyGUI.UIObjectFactory.GLoaderCreator>(L, 1)) 
                {
                    FairyGUI.UIObjectFactory.GLoaderCreator _creator = translator.GetDelegate<FairyGUI.UIObjectFactory.GLoaderCreator>(L, 1);
                    
                    FairyGUI.UIObjectFactory.SetLoaderExtension( _creator );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.UIObjectFactory.SetLoaderExtension!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    FairyGUI.UIObjectFactory.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NewObject_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<FairyGUI.ObjectType>(L, 1)) 
                {
                    FairyGUI.ObjectType _type;translator.Get(L, 1, out _type);
                    
                        var gen_ret = FairyGUI.UIObjectFactory.NewObject( _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<FairyGUI.PackageItem>(L, 1)&& translator.Assignable<System.Type>(L, 2)) 
                {
                    FairyGUI.PackageItem _pi = (FairyGUI.PackageItem)translator.GetObject(L, 1, typeof(FairyGUI.PackageItem));
                    System.Type _userClass = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        var gen_ret = FairyGUI.UIObjectFactory.NewObject( _pi, _userClass );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<FairyGUI.PackageItem>(L, 1)) 
                {
                    FairyGUI.PackageItem _pi = (FairyGUI.PackageItem)translator.GetObject(L, 1, typeof(FairyGUI.PackageItem));
                    
                        var gen_ret = FairyGUI.UIObjectFactory.NewObject( _pi );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.UIObjectFactory.NewObject!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
