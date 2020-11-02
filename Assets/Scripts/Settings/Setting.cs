namespace Assets.Scripts.Settings
{
    public class Setting<T> where T : class
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public bool Override { get; set; }
    }
}
