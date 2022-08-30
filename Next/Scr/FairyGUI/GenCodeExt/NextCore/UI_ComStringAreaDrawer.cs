namespace SkySwordKill.NextFGUI.NextCore
{
    public partial class UI_ComStringAreaDrawer
    {
        public void SetEditable(bool value)
        {
            grayed = !value;
            m_inContent.editable = value;
            m_btnEdit.enabled = value;
        }
    }
}