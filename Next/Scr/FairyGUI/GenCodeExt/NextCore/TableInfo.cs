namespace SkySwordKill.NextFGUI.NextCore
{
    public class TableInfo
    {
        public const float DEFAULT_GRID_WIDTH = 96f;

        public delegate string InfoGetter(object data);

        public TableInfo(string name, float width, InfoGetter getter)
        {
            Name = name;
            Width = width;
            Getter = getter;
        }

        public string Name { get; }
        public float Width { get; set; }
        public InfoGetter Getter { get; }
    }
}