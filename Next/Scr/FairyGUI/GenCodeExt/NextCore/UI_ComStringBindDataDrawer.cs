namespace SkySwordKill.NextFGUI.NextCore;

public partial class UI_ComStringBindDataDrawer
{
    public void SetEditable(bool value)
    {
        grayed = !value;
        m_inContent.enabled = value;
        m_btnEdit.enabled = value;
        m_inContent.cursor = value ? "text" : string.Empty;
    }
}