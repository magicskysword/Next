using System;
using FairyGUI;
using SkySwordKill.Next.Lua;
using XLua;

namespace SkySwordKill.Next.FGUI;

public class FGUIWindowLua : FGUIWindowBase
{
    public FGUIWindowLua(string pkgName, string comName, string scriptPath) : base(pkgName, comName)
    {
        this.ScriptPath = scriptPath;
        
        BindLuaTable();
    }
    
    public LuaTable Table;
    public string ScriptPath;

    public Action LuaCallback_OnInit;
    public Action LuaCallback_DoShowAnimation;
    public Action LuaCallback_DoHideAnimation;
    public Action LuaCallback_OnShown;
    public Action LuaCallback_OnHide;
    
    public Action LuaCallback_OnUpdate;
    public EventCallback1 LuaCallback_OnKeyDown;

    public Action OnCloseCallback;

    private void BindLuaTable()
    {
        Table = Main.Lua.RunScript<LuaTable>(ScriptPath);
        if(Table == null)
            throw new Exception($"Lua脚本加载失败：{ScriptPath}");
        
        Table.Set("script", this);
        Table.Set("contentPane", contentPane);
        
        Table.Get("OnInit", out LuaCallback_OnInit);
        Table.Get("DoShowAnimation", out LuaCallback_DoShowAnimation);
        Table.Get("DoHideAnimation", out LuaCallback_DoHideAnimation);
        Table.Get("OnShown", out LuaCallback_OnShown);
        Table.Get("OnHide", out LuaCallback_OnHide);
        Table.Get("OnUpdate", out LuaCallback_OnUpdate);
        Table.Get("OnKeyDown", out LuaCallback_OnKeyDown);
        
        Main.I.RegisterUpdateEvent(Update);
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        Main.I.UnRegisterUpdateEvent(Update);
        OnCloseCallback?.Invoke();
    }

    protected override void OnInit()
    {
        base.OnInit();
        
        LuaCallback_OnInit?.Invoke();
    }
    
    protected override void DoShowAnimation()
    {
        if (LuaCallback_DoShowAnimation != null)
        {
            LuaCallback_DoShowAnimation.Invoke();
        }
        else
        {
            base.DoShowAnimation();
        }
    }
    
    protected override void DoHideAnimation()
    {
        if (LuaCallback_DoHideAnimation != null)
        {
            LuaCallback_DoHideAnimation.Invoke();
        }
        else
        {
            base.DoHideAnimation();
        }
    }
    
    protected override void OnShown()
    {
        base.OnShown();
        
        LuaCallback_OnShown?.Invoke();
    }
    
    protected override void OnHide()
    {
        LuaCallback_OnHide?.Invoke();
        
        base.OnHide();
    }

    protected override void OnKeyDown(EventContext context)
    {
        LuaCallback_OnKeyDown?.Invoke(context);
    }

    protected void Update()
    {
        if (isShowing)
        {
            LuaCallback_OnUpdate?.Invoke();
        }
    }
}