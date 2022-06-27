namespace SkySwordKill.NextModEditor.Mod.Data
{
    public interface IModSeidDataGroup
    {
        ModSeidData GetSeid(int dataId, int seidId);
        ModSeidData GetOrCreateSeid(int dataId, int seidId);
        bool RemoveSeid(int dataId, int seidId);
        void ChangeSeidID(int oldDataId, int newDataId);
        void SwiftSeidID(int dataId1, int dataId2);
    }
}