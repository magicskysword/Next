using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog
{
    public class WindowWaitDialog : WindowDialogBase
    {
        private string _title;
        private string _showText;
        private Action _onWait;
        private Action<Action> _onWaitComplete;
        private Action _onComplete;
        private float _hideDelay;

        private WindowWaitDialog() : base("NextCore", "WinWaitDialog")
        {
        }

        /// <summary>
        /// 创建同步等待对话框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="showText"></param>
        /// <param name="hideDelay"></param>
        /// <param name="onWait"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static WindowWaitDialog CreateDialog(string title,string showText,float hideDelay = 1f,Action onWait = null,Action onComplete = null)
        {
            var window = new WindowWaitDialog();
            window._title = title;
            window._showText = showText;
            window._onWait = onWait;
            window._onComplete = onComplete;
            window._hideDelay = hideDelay;
            window.Show();

            return window;
        }
        
        /// <summary>
        /// 创建异步等待对话框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="showText"></param>
        /// <param name="hideDelay"></param>
        /// <param name="onWaitComplete"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static WindowWaitDialog CreateDialog(string title,string showText,float hideDelay = 1f,Action<Action> onWaitComplete = null,Action onComplete = null)
        {
            var window = new WindowWaitDialog();
            window._title = title;
            window._showText = showText;
            window._onWaitComplete = onWaitComplete;
            window._onComplete = onComplete;
            window._hideDelay = hideDelay;
            window.Show();

            return window;
        }
        
        public UI_WinWaitDialog MainView => (UI_WinWaitDialog)contentPane;
        
        protected override void OnInit()
        {
            base.OnInit();
            MainView.m_frame.title = _title;
            MainView.m_text.text = _showText;
        }

        protected override void OnShown()
        {
            base.OnShown();
            Timers.inst.Add(_hideDelay / 2f, 1, _ => OnWait());
        }

        private void OnWait()
        {
            if (_onWaitComplete != null)
            {
                _onWaitComplete.Invoke(() =>
                {
                    Timers.inst.Add(_hideDelay / 2f, 1, _ => Hide());
                });
            }
            else
            {
                _onWait?.Invoke();
                Timers.inst.Add(_hideDelay / 2f, 1, _ => Hide());
            }
        }

        protected override void OnHide()
        {
            base.OnHide();
            _onComplete?.Invoke();
        }
    }
}