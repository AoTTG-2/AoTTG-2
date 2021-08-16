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
        /// <summary>
        /// The level is not a custom map
        /// </summary>
        None,
        /// <summary>
        /// The custom level is an AssetBundle (loaded by .unity3d)
        /// </summary>
        AssetBundle,
        /// <summary>
        /// The custom level is an Custom Map (loaded by .txt)
        /// </summary>
        CustomMap,
        /// <summary>
        /// The custom level uses both the <see cref="AssetBundle"/> and <see cref="CustomMap"/>
        /// </summary>
        Hybrid
    }
}
