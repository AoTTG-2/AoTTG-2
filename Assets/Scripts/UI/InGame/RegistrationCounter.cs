public sealed class RegistrationCounter
{
    private int counter;

    public delegate void RegisteredHandler();

    public event RegisteredHandler FirstRegistered;

    public event RegisteredHandler LastUnregistered;

    public bool AnyRegistered => counter > 0;

    public void Register()
    {
        if (counter++ == 0)
            FirstRegistered?.Invoke();
    }

    public void Unregister()
    {
        if (--counter == 0)
            LastUnregistered?.Invoke();
    }
}