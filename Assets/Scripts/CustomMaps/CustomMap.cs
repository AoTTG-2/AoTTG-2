namespace Assets.Scripts.CustomMaps
{
    public struct CustomMap
    {
        public string Name { get; }
        public string Path { get; }

        public CustomMap(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }

    public enum CustomMapType
    {
        None,
        AssetBundle,
        CustomMap,
        Hybrid
    }
}
