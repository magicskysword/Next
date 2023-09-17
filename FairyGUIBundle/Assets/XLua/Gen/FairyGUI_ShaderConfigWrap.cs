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
    public class FairyGUIShaderConfigWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.ShaderConfig);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 19, 19);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetShader", _m_GetShader_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Get", _g_get_Get);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "imageShader", _g_get_imageShader);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "textShader", _g_get_textShader);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "bmFontShader", _g_get_bmFontShader);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "TMPFontShader", _g_get_TMPFontShader);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_ClipBox", _g_get_ID_ClipBox);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_ClipSoftness", _g_get_ID_ClipSoftness);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_AlphaTex", _g_get_ID_AlphaTex);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_StencilComp", _g_get_ID_StencilComp);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_Stencil", _g_get_ID_Stencil);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_StencilOp", _g_get_ID_StencilOp);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_StencilReadMask", _g_get_ID_StencilReadMask);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_ColorMask", _g_get_ID_ColorMask);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_ColorMatrix", _g_get_ID_ColorMatrix);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_ColorOffset", _g_get_ID_ColorOffset);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_BlendSrcFactor", _g_get_ID_BlendSrcFactor);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_BlendDstFactor", _g_get_ID_BlendDstFactor);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_ColorOption", _g_get_ID_ColorOption);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ID_Stencil2", _g_get_ID_Stencil2);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Get", _s_set_Get);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "imageShader", _s_set_imageShader);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "textShader", _s_set_textShader);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "bmFontShader", _s_set_bmFontShader);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "TMPFontShader", _s_set_TMPFontShader);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_ClipBox", _s_set_ID_ClipBox);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_ClipSoftness", _s_set_ID_ClipSoftness);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_AlphaTex", _s_set_ID_AlphaTex);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_StencilComp", _s_set_ID_StencilComp);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_Stencil", _s_set_ID_Stencil);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_StencilOp", _s_set_ID_StencilOp);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_StencilReadMask", _s_set_ID_StencilReadMask);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_ColorMask", _s_set_ID_ColorMask);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_ColorMatrix", _s_set_ID_ColorMatrix);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_ColorOffset", _s_set_ID_ColorOffset);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_BlendSrcFactor", _s_set_ID_BlendSrcFactor);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_BlendDstFactor", _s_set_ID_BlendDstFactor);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_ColorOption", _s_set_ID_ColorOption);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ID_Stencil2", _s_set_ID_Stencil2);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "FairyGUI.ShaderConfig does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetShader_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = FairyGUI.ShaderConfig.GetShader( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Get(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, FairyGUI.ShaderConfig.Get);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_imageShader(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, FairyGUI.ShaderConfig.imageShader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textShader(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, FairyGUI.ShaderConfig.textShader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bmFontShader(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, FairyGUI.ShaderConfig.bmFontShader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TMPFontShader(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, FairyGUI.ShaderConfig.TMPFontShader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_ClipBox(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_ClipBox);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_ClipSoftness(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_ClipSoftness);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_AlphaTex(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_AlphaTex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_StencilComp(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_StencilComp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_Stencil(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_Stencil);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_StencilOp(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_StencilOp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_StencilReadMask(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_StencilReadMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_ColorMask(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_ColorMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_ColorMatrix(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_ColorMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_ColorOffset(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_ColorOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_BlendSrcFactor(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_BlendSrcFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_BlendDstFactor(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_BlendDstFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_ColorOption(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_ColorOption);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID_Stencil2(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, FairyGUI.ShaderConfig.ID_Stencil2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Get(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    FairyGUI.ShaderConfig.Get = translator.GetDelegate<FairyGUI.ShaderConfig.GetFunction>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_imageShader(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.imageShader = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_textShader(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.textShader = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bmFontShader(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.bmFontShader = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TMPFontShader(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.TMPFontShader = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_ClipBox(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_ClipBox = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_ClipSoftness(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_ClipSoftness = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_AlphaTex(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_AlphaTex = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_StencilComp(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_StencilComp = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_Stencil(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_Stencil = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_StencilOp(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_StencilOp = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_StencilReadMask(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_StencilReadMask = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_ColorMask(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_ColorMask = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_ColorMatrix(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_ColorMatrix = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_ColorOffset(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_ColorOffset = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_BlendSrcFactor(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_BlendSrcFactor = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_BlendDstFactor(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_BlendDstFactor = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_ColorOption(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_ColorOption = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID_Stencil2(RealStatePtr L)
        {
		    try {
                
			    FairyGUI.ShaderConfig.ID_Stencil2 = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
