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
    public class FairyGUIMaterialManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.MaterialManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFlagsByKeywords", _m_GetFlagsByKeywords);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMaterial", _m_GetMaterial);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyMaterials", _m_DestroyMaterials);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshMaterials", _m_RefreshMaterials);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "onCreateNewMaterial", _e_onCreateNewMaterial);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "firstMaterialInFrame", _g_get_firstMaterialInFrame);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "firstMaterialInFrame", _s_set_firstMaterialInFrame);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "FairyGUI.MaterialManager does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFlagsByKeywords(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.IList<string> _keywords = (System.Collections.Generic.IList<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IList<string>));
                    
                        var gen_ret = gen_to_be_invoked.GetFlagsByKeywords( _keywords );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMaterial(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _flags = LuaAPI.xlua_tointeger(L, 2);
                    FairyGUI.BlendMode _blendMode;translator.Get(L, 3, out _blendMode);
                    uint _group = LuaAPI.xlua_touint(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.GetMaterial( _flags, _blendMode, _group );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyMaterials(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DestroyMaterials(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshMaterials(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RefreshMaterials(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_firstMaterialInFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.firstMaterialInFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_firstMaterialInFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.firstMaterialInFrame = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_onCreateNewMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			FairyGUI.MaterialManager gen_to_be_invoked = (FairyGUI.MaterialManager)translator.FastGetCSObj(L, 1);
                System.Action<UnityEngine.Material> gen_delegate = translator.GetDelegate<System.Action<UnityEngine.Material>>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action<UnityEngine.Material>!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.onCreateNewMaterial += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.onCreateNewMaterial -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.MaterialManager.onCreateNewMaterial!");
            return 0;
        }
        
		
		
    }
}
