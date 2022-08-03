using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlTextInputArea
    {
        public CtlTextInputArea(UI_ComInputArea inputArea)
        {
            InputArea = inputArea;
        }
        
        public UI_ComInputArea InputArea { get; }

        public string Text
        {
            get => InputArea.m_input.text;
            set => InputArea.m_input.text = value;
        }
    }
}