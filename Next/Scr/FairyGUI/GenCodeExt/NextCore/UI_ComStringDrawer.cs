﻿namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComStringDrawer
{
    public void SetEditable(bool value)
    {
        grayed = !value;
        m_inContent.editable = value;
        m_inContent.cursor = value ? "text" : string.Empty;
    }
}