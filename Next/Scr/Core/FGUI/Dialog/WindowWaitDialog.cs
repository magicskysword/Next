using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Dialog;

public class WaitContext
{
    public object Data { get; set; }
    public string Message { get; set; }
    public Exception Exception { get; set; }
}
    
public class WindowWaitDialog : WindowDialogBase
{
    private string _title;
    private string _showText;
    private Action<WaitContext> _onWait;
    private Action<Action, WaitContext> _onWaitAsync;
    private Action<WaitContext> _onComplete;
    private float _hideDelay;
    private WaitContext _context = new WaitContext();

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
    public static WindowWaitDialog CreateDialog(string title,string showText,float hideDelay = 1f,Action<WaitContext> onWait = null,Action<WaitContext> onComplete = null)
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
    public static WindowWaitDialog CreateDialogAsync(string title,string showText,float hideDelay = 1f,
        Action<Action, WaitContext> onWaitComplete = null,Action<WaitContext> onComplete = null)
    {
        var window = new WindowWaitDialog();
        window._title = title;
        window._showText = showText;
        window._onWaitAsync = onWaitComplete;
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
        if (_onWaitAsync != null)
        {
            _onWaitAsync.Invoke(() =>
            {
                Timers.inst.Add(_hideDelay / 2f, 1, _ => Hide());
            }, _context);
        }
        else
        {
            _onWait?.Invoke(_context);
            Timers.inst.Add(_hideDelay / 2f, 1, _ => Hide());
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        _onComplete?.Invoke(_context);
    }
}