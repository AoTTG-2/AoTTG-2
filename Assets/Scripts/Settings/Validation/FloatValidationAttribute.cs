namespace Assets.Scripts.Settings.Validation
{
    public class FloatValidationAttribute : ValidationAttribute
    {
        public float MinValue { get; }
        public float MaxValue { get; }
        public float Default { get; }

        public FloatValidationAttribute(float minValue, float maxValue, float @default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Default = @default;
        }
    }
}
