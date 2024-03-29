﻿using System;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using SkySwordKill.NextModEditor.Mod;
using UnityEngine;

namespace SkySwordKill.Next.FGUI;

public static class FGUIUtils
{
    public static T As<T>(this GObject obj) where T : GObject
    {
        //Main.LogDebug($"Type : {component.GetType()} to Type : {typeof(T)}");
        return obj as T;
    }

    public static void BindHSeg(GObject seg, Func<float> xMinGetter, Func<float> xMaxGetter)
    {
        void OnDragMove(EventContext context)
        {
            var pos = new Vector2(context.inputEvent.x, 0);
            seg.x = Mathf.Clamp(seg.parent.RootToLocal(pos, null).x, xMinGetter.Invoke(), xMaxGetter.Invoke());
        }

        void OnDragStart(EventContext context)
        {
            context.PreventDefault();
            DragDropManager.inst.StartDrag(null, null, null, (int)context.data);

            DragDropManager.inst.dragAgent.onDragMove.Set((EventCallback1)OnDragMove);
        }

        seg.draggable = true;
        seg.onDragStart.Set((EventCallback1)OnDragStart);
        seg.cursor = FGUIManager.MOUSE_RESIZE_H;
    }

    public static void BindIntEndEdit(this GTextInput input, Action<int> endEdit, Action<string> onParseFail = null)
    {
        input.restrict = "[0-9-]";
        input.onFocusOut.Set(() =>
        {
            var str = input.text;
            if(int.TryParse(str,out var num))
            {
                endEdit?.Invoke(num);
            }
            else
            {
                onParseFail?.Invoke(str);
            }
        });
    }
        
    public static void BindFloatEndEdit(this GTextInput input,Action<float> endEdit, Action<string> onParseFail = null)
    {
        input.restrict = "[0-9-.eE]";
        input.onFocusOut.Set(() =>
        {
            var str = input.text;
            if(float.TryParse(str,out var num))
            {
                endEdit?.Invoke(num);
            }
            else
            {
                onParseFail?.Invoke(str);
            }
        });
    }
    
    public static void BindLongEndEdit(this GTextInput input,Action<long> endEdit, Action<string> onParseFail = null)
    {
        input.restrict = "[0-9-]";
        input.onFocusOut.Set(() =>
        {
            var str = input.text;
            if(long.TryParse(str,out var num))
            {
                endEdit?.Invoke(num);
            }
            else
            {
                onParseFail?.Invoke(str);
            }
        });
    }
    
    public static void BindDoubleEndEdit(this GTextInput input,Action<double> endEdit, Action<string> onParseFail = null)
    {
        input.restrict = "[0-9-.eE]";
        input.onFocusOut.Set(() =>
        {
            var str = input.text;
            if(double.TryParse(str,out var num))
            {
                endEdit?.Invoke(num);
            }
            else
            {
                onParseFail?.Invoke(str);
            }
        });
    }
        
    public static void BindIntArrayEndEdit(this GTextInput input,Action<List<int>> endEdit, Action<string> onParseFail = null)
    {
        input.restrict = "[0-9-,，]";
        input.onFocusOut.Set(() =>
        {
            var strArr = input.text;
            if(strArr.TryFormatToListInt(out var list))
            {
                endEdit?.Invoke(list);
            }
            else
            {
                onParseFail?.Invoke(strArr);
            }
        });
    }
    
    /// <summary>
    /// 不要手动创建NTexture，会造成内存泄露
    /// 自动创建并设置销毁模式的NTexture
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static NTexture CreateTempNTexture(this Texture2D texture)
    {
        var nTex = new NTexture(texture);
        nTex.onRelease += OnReleaseNTexture;
        nTex.destroyMethod = DestroyMethod.Destroy;
        return nTex;
    }
    
    private static void OnReleaseNTexture(NTexture obj)
    {
        obj.Dispose();
    }   

    public class HyLabel
    {
        public string Href { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool HasUnderline = true;
        public string HoverColor { get; set; } = string.Empty;
    }

    public static string UHyperTextToFGUIText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        
        StringBuilder sb = new StringBuilder();
        // 计算font标签数量，用于关闭标签
        var fLabel = 0;
        for (int i = 0; i < text.Length; i++)
        {
            var c = text[i];
            if (c == '#' && i + 1 < text.Length)
            {
                var c2 = text[i + 1];
                switch (c2)
                {
                    case '#':
                        sb.Append('#');
                        i += 1;
                        break;
                    case 'R':
                        sb.Append("<font color=#FF0000>");
                        i += 1;
                        fLabel += 1;
                        break;
                    case 'G':
                        sb.Append("<font color=#00FF00>");
                        i += 1;
                        fLabel += 1;
                        break;
                    case 'B':
                        sb.Append("<font color=#0000FF>");
                        i += 1;
                        fLabel += 1;
                        break;
                    case 'K':
                        sb.Append("<font color=#000000>");
                        i += 1;
                        fLabel += 1;
                        break;
                    case 'Y':
                        sb.Append("<font color=#FFFF00>");
                        i += 1;
                        fLabel += 1;
                        break;
                    case 'W':
                        sb.Append("<font color=#FFFFFF>");
                        i += 1;
                        fLabel += 1;
                        break;
                    case 'c':
                        if(i + 7 < text.Length)
                        {
                            var color = text.Substring(i + 2, 6);
                            sb.Append($"<font color=#{color}>");
                            i += 7;
                            fLabel += 1;
                        }
                        break;
                    case 'n':
                        for (int j = 0; j < fLabel; j++)
                        {
                            sb.Append("</font>");
                        }

                        i += 1;
                        fLabel = 0;
                        break;
                    case 'r':
                        sb.Append("\n");
                        break;
                }
            }
            else if (c == '<')
            {
                var closeIndex = text.IndexOf('>', i);
                if (closeIndex >= 0)
                {
                    var labelText = text.Substring(i + 1, closeIndex - i - 2);
                    var labelInfos = ParseLabel(labelText);
                    if (labelInfos.Count > 0)
                    {
                        switch (labelInfos[0].LabelName)
                        {
                            case "hy":
                                var hyLabel = new HyLabel();
                                foreach (var labelInfo in labelInfos)
                                {
                                    switch (labelInfo.LabelName)
                                    {
                                        case "t":
                                            hyLabel.Text = labelInfo.Content;
                                            break;
                                        case "l":
                                            hyLabel.Href = $"GuideLink://{labelInfo.Content}";
                                            break;
                                        case "ul":
                                            hyLabel.HasUnderline = labelInfo.Content != "0";
                                            break;
                                        case "fhc":
                                            hyLabel.HoverColor = labelInfo.Content;
                                            break;
                                    }
                                }

                                sb.Append($"<a href='{hyLabel.Href}'");
                                if (!string.IsNullOrWhiteSpace(hyLabel.HoverColor))
                                    sb.Append($" hc='{hyLabel.HoverColor}'");
                                sb.Append(">");
                                if (hyLabel.HasUnderline)
                                    sb.Append($"<u>");
                                sb.Append(hyLabel.Text);
                                if (hyLabel.HasUnderline)
                                    sb.Append($"</u>");
                                sb.Append("</a>");
                                break;
                        }
                    }
                    i = closeIndex;
                }
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public class LabelInfo
    {
        public string LabelName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
        
    public static List<LabelInfo> ParseLabel(string labelText)
    {
        var list = new List<LabelInfo>();
        foreach (var str in labelText.Split(' '))
        {
            if(string.IsNullOrEmpty(str))
                continue;
            var arr = str.Split('=');
            var labelInfo = new LabelInfo();
            labelInfo.LabelName = arr[0];
            if (arr.Length >= 2)
            {
                labelInfo.Content = arr[1];
            }

            list.Add(labelInfo);
        }

        return list;
    }
}