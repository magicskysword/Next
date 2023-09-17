using System;
using System.Collections;
using System.Collections.Generic;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.FGUI.Dialog;
using XLua;

namespace SkySwordKill.Next;

/// <summary>
/// Next方法快捷入口
/// </summary>
public static class Helper
{
    /// <summary>
    /// 运行指定的剧情事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void StartEvent(string eventName, Action callback)
    {
        if(callback != null)
            DialogAnalysis.OnDialogComplete += callback;
        DialogAnalysis.StartDialogEvent(eventName);
    }

    /// <summary>
    /// 运行多行剧情事件脚本
    /// </summary>
    /// <param name="scripts"></param>
    /// <param name="callback"></param>
    public static void RunScript(string scripts, Action callback)
    {
        if(callback != null)
            DialogAnalysis.OnDialogComplete += callback;
        DialogAnalysis.StartTestDialogEvent(scripts);
    }

    /// <summary>
    /// 获取运行时脚本的结果
    /// 不需要添加周围的符号，例如<code>EvaluateScript("GetInt(\"变量1\")")</code>
    /// Lua里调用为CS.SkySwordKill.Next.Helper.EvaluateScript([[GetInt("变量1")]])
    /// </summary>
    /// <param name="script"></param>
    /// <returns></returns>
    public static object EvaluateScript(string script)
    {
        var evaluator = DialogAnalysis.GetEvaluate(null);
        return evaluator.Evaluate(script);
    }

    /// <summary>
    /// 获取整数值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int GetInt(string key)
    {
        return DialogAnalysis.GetInt(key);
    }
    
    /// <summary>
    /// 设置整数值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetInt(string key,int value)
    {
        DialogAnalysis.SetInt(key,value);
    }
    
    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetStr(string key)
    {
        return DialogAnalysis.GetStr(key);
    }
    
    /// <summary>
    /// 设置字符串
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetStr(string key,string value)
    {
        DialogAnalysis.SetStr(key,value);
    }
    
    /// <summary>
    /// 显示一个确认窗口
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="callback"></param>
    public static void ShowConfirmWindow(string title, string content, Action<bool> callback)
    {
        WindowConfirmDialog.CreateDialog(title, content, true, 
            () => callback?.Invoke(true) ,
            () => callback?.Invoke(false));
    }
    
    /// <summary>
    /// 显示一个提示窗口
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="callback"></param>
    public static void ShowTipWindow(string title,string content, Action callback)
    {
        WindowConfirmDialog.CreateDialog(title, content, false, () => callback?.Invoke());
    }
    
    /// <summary>
    /// 显示一个文本输入窗口
    /// </summary>
    /// <param name="title"></param>
    /// <param name="defaultText"></param>
    /// <param name="callback"></param>
    public static void ShowInputStringWindow(string title, string defaultText,  Action<string> callback)
    {
        WindowStringInputDialog.CreateDialog(title, defaultText, false, str =>
        {
            callback?.Invoke(str);
        });
    }
    
    /// <summary>
    /// 显示一个整数输入窗口
    /// </summary>
    /// <param name="title"></param>
    /// <param name="defaultInt"></param>
    /// <param name="callback"></param>
    public static void ShowInputIntWindow(string title, int defaultInt,  Action<int> callback)
    {
        WindowIntInputDialog.CreateDialog(title, false, defaultInt, val =>
        {
            callback?.Invoke(val);
        });
    }

    /// <summary>
    /// 打开一个绑定了Lua脚本的FGUI
    /// </summary>
    /// <param name="guiRes"></param>
    /// <param name="guiCom"></param>
    /// <param name="guiScript"></param>
    /// <param name="modal"></param>
    /// <param name="callback"></param>
    public static void OpenGUI(string guiRes, string guiCom, string guiScript, bool modal, Action callback)
    {
        var fguiWindow = new FGUIWindowLua(guiRes, guiCom, guiScript);
        fguiWindow.OnCloseCallback = callback;
        fguiWindow.modal = modal;
        fguiWindow.Show();
        fguiWindow.Center();
    }

    public static List<string> ToStrList(LuaTable table)
    {
        var list = new List<string>();
        foreach (var item in table.GetKeys())
        {
            list.Add(table.Get<object, object>(item).ToString());
        }
        return list;
    }
    
    public static List<int> ToIntList(LuaTable table)
    {
        var list = new List<int>();
        foreach (var item in table.GetKeys())
        {
            list.Add(int.Parse(table.Get<object, object>(item).ToString()));
        }
        return list;
    }
    
    public static string[] ToStrArray(LuaTable table)
    {
        var list = new List<string>();
        foreach (var item in table.GetKeys())
        {
            list.Add(table.Get<object, object>(item).ToString());
        }
        return list.ToArray();
    }
    
    public static int[] ToIntArray(LuaTable table)
    {
        var list = new List<int>();
        foreach (var item in table.GetKeys())
        {
            list.Add(int.Parse(table.Get<object, object>(item).ToString()));
        }
        return list.ToArray();
    }
}