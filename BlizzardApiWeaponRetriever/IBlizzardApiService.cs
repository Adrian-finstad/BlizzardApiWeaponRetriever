namespace BlizzardApiWeaponRetriever
{
    public interface IBlizzardApiService
    {
        Task<ItemClassesIndex> GetItemClasses(string item);

        public record ItemClassesIndex(string Id, string Name, string Ilvl);
    }
}
