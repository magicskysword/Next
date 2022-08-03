using FairyGUI;
using SkySwordKill.Next.XiaoYeGUI;
using UnityEngine;

namespace SkySwordKill.Next.FGUI
{
    public class FGUIPanelBase
    {
        public string comName;
        public string pkgName;
        public RayBlocker RayBlocker;
        
        public GComponent contentPane { get; set; }
        public bool isShowing { get; set; }
        private bool isInit { get; set; } = false;

        public FGUIPanelBase(string pkgName,string comName)
        {
            this.pkgName = pkgName;
            this.comName = comName;
            contentPane = Main.FGUI.CreateUIObject(pkgName, comName).asCom;
        }
        
        protected virtual void OnInit()
        {
            RayBlocker = RayBlocker.CreateRayBlock(comName);
            contentPane.onSizeChanged.Add(ResetRayBlocker);
            contentPane.onPositionChanged.Add(ResetRayBlocker);
        }

        public virtual void ResetRayBlocker()
        {
            RayBlocker.SetSize(new Rect(contentPane.x, contentPane.y, contentPane.width, contentPane.height));
        }
        
        public void Show()
        {
            if(!isInit)
            {
                OnInit();
                isInit = true;
            }
            GRoot.inst.AddChild(contentPane);
            DoShowAnimation();
        }
        
        public void Hide()
        {
            if (isShowing)
                DoHideAnimation();
        }

        protected virtual void DoShowAnimation()
        {
            OnShown();
        }
        
        protected virtual void DoHideAnimation()
        {
            HideImmediately();
        }

        protected virtual void HideImmediately()
        {
            GRoot.inst.RemoveChild(contentPane);
            isShowing = false;
            OnHide();
        }

        protected virtual void OnShown()
        {
            isShowing = true;
            RayBlocker.OpenBlocker();
        }

        protected virtual void OnHide()
        {
            RayBlocker.CloseBlocker();
            RayBlocker.DestroySelf();
            contentPane.Dispose();
        }
        
        /// <summary>
        /// 按缩放因子设置缩放大小并居中
        /// </summary>
        public void MakeFullScreenAndCenter(float factor = 1f)
        {
            contentPane.SetSize(GRoot.inst.width * factor, GRoot.inst.height * factor);
            contentPane.Center();
            ResetRayBlocker();
        }
    }
}