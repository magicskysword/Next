using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlTextInputArea
{
    public CtlTextInputArea(UI_ComInputArea inputArea)
    {
        InputArea = inputArea;
        InputArea.m_input.cursor = FGUIManager.MOUSE_TEXT;
        InputArea.m_input.onChanged.Add(OnInputChanged);
    }
        
    private void OnInputChanged(EventContext context)
    {
        OnChanged?.Invoke(InputArea.m_input.text);
    }

    public UI_ComInputArea InputArea { get; }

    public string Text
    {
        get => InputArea.m_input.text;
        set => InputArea.m_input.text = value;
    }

    public event Action<string> OnChanged;
}