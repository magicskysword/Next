using System;
using FairyGUI;
using FairyGUI.Utils;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlTextPreviewArea
{
    public CtlTextPreviewArea(UI_ComTextPreviewArea preview)
    {
        MainView = preview;
        MainView.m_txtPreview.richTextField.RefreshObjects();
        var options = MainView.m_txtPreview.richTextField.htmlParseOptions;
        options.linkUnderline = false;
    }

    public UI_ComTextPreviewArea MainView { get; private set; }
    public event Func<string, string> OnAnalysisRef;

    public void SetPreviewText(string txt)
    {
        var transTxt = FGUIUtils.UHyperTextToFGUIText(txt);
        MainView.m_txtPreview.text = transTxt;
        var richTextField = MainView.m_txtPreview.richTextField;
        int cnt = richTextField.htmlElementCount;
        for (int i = 0; i < cnt; i++)
        {
            var element = richTextField.GetHtmlElementAt(i);
            if (element.htmlObject is HtmlLink link)
            {
                link.displayObject.onRollOver.Add(() =>
                {
                    var refData = element.GetString("href");
                    if(!string.IsNullOrEmpty(refData))
                    {
                        if(OnAnalysisRef != null)
                        {
                            var refText = OnAnalysisRef(refData);
                            if(!string.IsNullOrEmpty(refText))
                            {
                                GRoot.inst.ShowTooltips(refText);
                            }
                        }
                        else
                        {
                            GRoot.inst.ShowTooltips(refData);
                        }
                    }
                });
                link.displayObject.onRollOut.Add(() =>
                {
                    GRoot.inst.HideTooltips();
                });
            }
        }
    }
}