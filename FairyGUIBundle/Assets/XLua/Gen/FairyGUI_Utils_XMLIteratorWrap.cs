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
    public class FairyGUIUtilsXMLIteratorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.Utils.XMLIterator);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 12, 3, 3);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Begin", _m_Begin_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NextTag", _m_NextTag_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTagSource", _m_GetTagSource_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRawText", _m_GetRawText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetText", _m_GetText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HasAttribute", _m_HasAttribute_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAttribute", _m_GetAttribute_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAttributeInt", _m_GetAttributeInt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAttributeFloat", _m_GetAttributeFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAttributeBool", _m_GetAttributeBool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAttributes", _m_GetAttributes_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "tagName", _g_get_tagName);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "tagType", _g_get_tagType);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "lastTagName", _g_get_lastTagName);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "tagName", _s_set_tagName);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "tagType", _s_set_tagType);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "lastTagName", _s_set_lastTagName);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new FairyGUI.Utils.XMLIterator();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Begin_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    string _source = LuaAPI.lua_tostring(L, 1);
                    bool _lowerCaseName = LuaAPI.lua_toboolean(L, 2);
                    
                    FairyGUI.Utils.XMLIterator.Begin( _source, _lowerCaseName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _source = LuaAPI.lua_tostring(L, 1);
                    
                    FairyGUI.Utils.XMLIterator.Begin( _source );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.Begin!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextTag_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.NextTag(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTagSource_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetTagSource(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRawText_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool _trim = LuaAPI.lua_toboolean(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetRawText( _trim );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 0) 
                {
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetRawText(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetRawText!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetText_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool _trim = LuaAPI.lua_toboolean(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetText( _trim );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 0) 
                {
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetText(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetText!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasAttribute_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.HasAttribute( _attrName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttribute_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttribute( _attrName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    string _defValue = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttribute( _attrName, _defValue );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetAttribute!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttributeInt_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributeInt( _attrName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    int _defValue = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributeInt( _attrName, _defValue );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetAttributeInt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttributeFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributeFloat( _attrName );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    float _defValue = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributeFloat( _attrName, _defValue );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetAttributeFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttributeBool_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributeBool( _attrName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    string _attrName = LuaAPI.lua_tostring(L, 1);
                    bool _defValue = LuaAPI.lua_toboolean(L, 2);
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributeBool( _attrName, _defValue );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetAttributeBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAttributes_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Collections.Generic.Dictionary<string, string>>(L, 1)) 
                {
                    System.Collections.Generic.Dictionary<string, string> _result = (System.Collections.Generic.Dictionary<string, string>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<string, string>));
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributes( _result );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<System.Collections.Hashtable>(L, 1)) 
                {
                    System.Collections.Hashtable _result = (System.Collections.Hashtable)translator.GetObject(L, 1, typeof(System.Collections.Hashtable));
                    
                        var gen_ret = FairyGUI.Utils.XMLIterator.GetAttributes( _result );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.Utils.XMLIterator.GetAttributes!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tagName(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, FairyGUI.Utils.XMLIterator.tagName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tagType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, FairyGUI.Utils.XMLIterator.tagType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lastTagName(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, FairyGUI.Utils.XMLIterator.lastTagName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tagName(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.Utils.XMLIterator.tagName = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tagType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			FairyGUI.Utils.XMLTagType gen_value;translator.Get(L, 1, out gen_value);
				FairyGUI.Utils.XMLIterator.tagType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lastTagName(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.Utils.XMLIterator.lastTagName = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
