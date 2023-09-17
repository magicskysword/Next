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
    public class FairyGUIGTreeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(FairyGUI.GTree);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 5, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSelectedNode", _m_GetSelectedNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSelectedNodes", _m_GetSelectedNodes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SelectNode", _m_SelectNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnselectNode", _m_UnselectNode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ExpandAll", _m_ExpandAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CollapseAll", _m_CollapseAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Setup_BeforeAdd", _m_Setup_BeforeAdd);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "rootNode", _g_get_rootNode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "indent", _g_get_indent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clickToExpand", _g_get_clickToExpand);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "treeNodeRender", _g_get_treeNodeRender);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "treeNodeWillExpand", _g_get_treeNodeWillExpand);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "indent", _s_set_indent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "clickToExpand", _s_set_clickToExpand);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "treeNodeRender", _s_set_treeNodeRender);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "treeNodeWillExpand", _s_set_treeNodeWillExpand);
            
			
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
					
					var gen_ret = new FairyGUI.GTree();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GTree constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSelectedNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSelectedNode(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSelectedNodes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSelectedNodes(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<FairyGUI.GTreeNode>>(L, 2)) 
                {
                    System.Collections.Generic.List<FairyGUI.GTreeNode> _result = (System.Collections.Generic.List<FairyGUI.GTreeNode>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<FairyGUI.GTreeNode>));
                    
                        var gen_ret = gen_to_be_invoked.GetSelectedNodes( _result );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GTree.GetSelectedNodes!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SelectNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<FairyGUI.GTreeNode>(L, 2)) 
                {
                    FairyGUI.GTreeNode _node = (FairyGUI.GTreeNode)translator.GetObject(L, 2, typeof(FairyGUI.GTreeNode));
                    
                    gen_to_be_invoked.SelectNode( _node );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<FairyGUI.GTreeNode>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    FairyGUI.GTreeNode _node = (FairyGUI.GTreeNode)translator.GetObject(L, 2, typeof(FairyGUI.GTreeNode));
                    bool _scrollItToView = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SelectNode( _node, _scrollItToView );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GTree.SelectNode!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnselectNode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.GTreeNode _node = (FairyGUI.GTreeNode)translator.GetObject(L, 2, typeof(FairyGUI.GTreeNode));
                    
                    gen_to_be_invoked.UnselectNode( _node );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExpandAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ExpandAll(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<FairyGUI.GTreeNode>(L, 2)) 
                {
                    FairyGUI.GTreeNode _folderNode = (FairyGUI.GTreeNode)translator.GetObject(L, 2, typeof(FairyGUI.GTreeNode));
                    
                    gen_to_be_invoked.ExpandAll( _folderNode );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GTree.ExpandAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CollapseAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.CollapseAll(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<FairyGUI.GTreeNode>(L, 2)) 
                {
                    FairyGUI.GTreeNode _folderNode = (FairyGUI.GTreeNode)translator.GetObject(L, 2, typeof(FairyGUI.GTreeNode));
                    
                    gen_to_be_invoked.CollapseAll( _folderNode );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to FairyGUI.GTree.CollapseAll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Setup_BeforeAdd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    FairyGUI.Utils.ByteBuffer _buffer = (FairyGUI.Utils.ByteBuffer)translator.GetObject(L, 2, typeof(FairyGUI.Utils.ByteBuffer));
                    int _beginPos = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.Setup_BeforeAdd( _buffer, _beginPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rootNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.rootNode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_indent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.indent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clickToExpand(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.clickToExpand);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_treeNodeRender(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.treeNodeRender);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_treeNodeWillExpand(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.treeNodeWillExpand);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_indent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.indent = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clickToExpand(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.clickToExpand = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_treeNodeRender(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.treeNodeRender = translator.GetDelegate<FairyGUI.GTree.TreeNodeRenderDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_treeNodeWillExpand(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                FairyGUI.GTree gen_to_be_invoked = (FairyGUI.GTree)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.treeNodeWillExpand = translator.GetDelegate<FairyGUI.GTree.TreeNodeWillExpandDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
