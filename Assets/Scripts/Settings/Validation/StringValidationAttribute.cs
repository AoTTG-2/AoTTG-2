namespace Assets.Scripts.Settings.Validation
{
    public class StringValidationAttribute : ValidationAttribute
    {
        public int MaxLength { get; }

        public StringValidationAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
    }
}
