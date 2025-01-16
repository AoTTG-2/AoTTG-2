namespace Assets.Scripts.Settings.Validation
{
    public class IntValidationAttribute : ValidationAttribute
    {
        public int MinValue { get; }
        public int MaxValue { get; }
        public int Default { get; }

        public IntValidationAttribute(int minValue, int maxValue, int @default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Default = @default;
        }
    }
}
