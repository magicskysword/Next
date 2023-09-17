#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(FairyGUI.BlendModeUtils), FairyGUIBlendModeUtilsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.CaptureCamera), FairyGUICaptureCameraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Container), FairyGUIContainerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.DisplayObject), FairyGUIDisplayObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.DisplayObjectInfo), FairyGUIDisplayObjectInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GoWrapper), FairyGUIGoWrapperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ColliderHitTest), FairyGUIColliderHitTestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.HitTestContext), FairyGUIHitTestContextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.MeshColliderHitTest), FairyGUIMeshColliderHitTestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PixelHitTestData), FairyGUIPixelHitTestDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PixelHitTest), FairyGUIPixelHitTestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RectHitTest), FairyGUIRectHitTestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ShapeHitTest), FairyGUIShapeHitTestWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Image), FairyGUIImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.MaterialManager), FairyGUIMaterialManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.CompositeMesh), FairyGUICompositeMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.EllipseMesh), FairyGUIEllipseMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.FillMesh), FairyGUIFillMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.LineMesh), FairyGUILineMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PlaneMesh), FairyGUIPlaneMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PolygonMesh), FairyGUIPolygonMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RectMesh), FairyGUIRectMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RegularPolygonMesh), FairyGUIRegularPolygonMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RoundedRectMesh), FairyGUIRoundedRectMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.StraightLineMesh), FairyGUIStraightLineMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.VertexBuffer), FairyGUIVertexBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.MovieClip), FairyGUIMovieClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.NAudioClip), FairyGUINAudioClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.NGraphics), FairyGUINGraphicsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.NTexture), FairyGUINTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ShaderConfig), FairyGUIShaderConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Shape), FairyGUIShapeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Stage), FairyGUIStageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.StageCamera), FairyGUIStageCameraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.StageEngine), FairyGUIStageEngineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Stats), FairyGUIStatsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.BaseFont), FairyGUIBaseFontWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.BitmapFont), FairyGUIBitmapFontWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.DynamicFont), FairyGUIDynamicFontWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Emoji), FairyGUIEmojiWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.FontManager), FairyGUIFontManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.InputTextField), FairyGUIInputTextFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RichTextField), FairyGUIRichTextFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RTLSupport), FairyGUIRTLSupportWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.SelectionShape), FairyGUISelectionShapeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TextField), FairyGUITextFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TextFormat), FairyGUITextFormatWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TouchScreenKeyboard), FairyGUITouchScreenKeyboardWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TypingEffect), FairyGUITypingEffectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UpdateContext), FairyGUIUpdateContextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.EventContext), FairyGUIEventContextWrap.__Register);
        
        }
        
        static void wrapInit1(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(FairyGUI.EventDispatcher), FairyGUIEventDispatcherWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.EventListener), FairyGUIEventListenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.InputEvent), FairyGUIInputEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.BlurFilter), FairyGUIBlurFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ColorFilter), FairyGUIColorFilterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.LongPressGesture), FairyGUILongPressGestureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PinchGesture), FairyGUIPinchGestureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.RotationGesture), FairyGUIRotationGestureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.SwipeGesture), FairyGUISwipeGestureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.EaseManager), FairyGUIEaseManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.CustomEase), FairyGUICustomEaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GPathPoint), FairyGUIGPathPointWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GPath), FairyGUIGPathWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GTween), FairyGUIGTweenWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GTweener), FairyGUIGTweenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TweenValue), FairyGUITweenValueWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ChangePageAction), FairyGUIChangePageActionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ControllerAction), FairyGUIControllerActionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PlayTransitionAction), FairyGUIPlayTransitionActionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.AsyncCreationHelper), FairyGUIAsyncCreationHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Controller), FairyGUIControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.DragDropManager), FairyGUIDragDropManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.EMRenderSupport), FairyGUIEMRenderSupportWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GButton), FairyGUIGButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GComboBox), FairyGUIGComboBoxWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GComponent), FairyGUIGComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearAnimation), FairyGUIGearAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearBase), FairyGUIGearBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearTweenConfig), FairyGUIGearTweenConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearColor), FairyGUIGearColorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearDisplay), FairyGUIGearDisplayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearDisplay2), FairyGUIGearDisplay2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearFontSize), FairyGUIGearFontSizeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearIcon), FairyGUIGearIconWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearLook), FairyGUIGearLookWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearSize), FairyGUIGearSizeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearText), FairyGUIGearTextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GearXY), FairyGUIGearXYWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GGraph), FairyGUIGGraphWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GGroup), FairyGUIGGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GImage), FairyGUIGImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GLabel), FairyGUIGLabelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GList), FairyGUIGListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GLoader), FairyGUIGLoaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GLoader3D), FairyGUIGLoader3DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GMovieClip), FairyGUIGMovieClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GObject), FairyGUIGObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GObjectPool), FairyGUIGObjectPoolWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GProgressBar), FairyGUIGProgressBarWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GRichTextField), FairyGUIGRichTextFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GRoot), FairyGUIGRootWrap.__Register);
        
        }
        
        static void wrapInit2(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(FairyGUI.GScrollBar), FairyGUIGScrollBarWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GSlider), FairyGUIGSliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GTextField), FairyGUIGTextFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GTextInput), FairyGUIGTextInputWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GTree), FairyGUIGTreeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GTreeNode), FairyGUIGTreeNodeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Margin), FairyGUIMarginWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PackageItem), FairyGUIPackageItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.PopupMenu), FairyGUIPopupMenuWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Relations), FairyGUIRelationsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.ScrollPane), FairyGUIScrollPaneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Transition), FairyGUITransitionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TranslationHelper), FairyGUITranslationHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIConfig), FairyGUIUIConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIContentScaler), FairyGUIUIContentScalerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIObjectFactory), FairyGUIUIObjectFactoryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIPackage), FairyGUIUIPackageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIPainter), FairyGUIUIPainterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIPanel), FairyGUIUIPanelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Window), FairyGUIWindowWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Timers), FairyGUITimersWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.ByteBuffer), FairyGUIUtilsByteBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlButton), FairyGUIUtilsHtmlButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlElement), FairyGUIUtilsHtmlElementWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlImage), FairyGUIUtilsHtmlImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlInput), FairyGUIUtilsHtmlInputWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlLink), FairyGUIUtilsHtmlLinkWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlPageContext), FairyGUIUtilsHtmlPageContextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlParseOptions), FairyGUIUtilsHtmlParseOptionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlParser), FairyGUIUtilsHtmlParserWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.HtmlSelect), FairyGUIUtilsHtmlSelectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.ToolSet), FairyGUIUtilsToolSetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.UBBParser), FairyGUIUtilsUBBParserWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.XML), FairyGUIUtilsXMLWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.XMLIterator), FairyGUIUtilsXMLIteratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.XMLList), FairyGUIUtilsXMLListWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.XMLUtils), FairyGUIUtilsXMLUtilsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.ZipReader), FairyGUIUtilsZipReaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.BlendModeUtils.BlendFactor), FairyGUIBlendModeUtilsBlendFactorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.GoWrapper.RendererInfo), FairyGUIGoWrapperRendererInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.MovieClip.Frame), FairyGUIMovieClipFrameWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.NGraphics.VertexMatrix), FairyGUINGraphicsVertexMatrixWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.BitmapFont.BMGlyph), FairyGUIBitmapFontBMGlyphWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TextField.LineInfo), FairyGUITextFieldLineInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TextField.LineCharInfo), FairyGUITextFieldLineCharInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.TextField.CharPosition), FairyGUITextFieldCharPositionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UpdateContext.ClipInfo), FairyGUIUpdateContextClipInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.UIConfig.ConfigValue), FairyGUIUIConfigConfigValueWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.XMLList.Enumerator), FairyGUIUtilsXMLListEnumeratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(FairyGUI.Utils.ZipReader.ZipEntry), FairyGUIUtilsZipReaderZipEntryWrap.__Register);
        
        
        
        }
        
        static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            wrapInit1(luaenv, translator);
            
            wrapInit2(luaenv, translator);
            
            
        }
        
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter(Init);
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
