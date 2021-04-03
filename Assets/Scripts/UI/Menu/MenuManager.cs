public static class MenuManager
{
    private static readonly RegistrationCounter menuCounter = new RegistrationCounter();

    public static event RegistrationCounter.RegisteredHandler MenuClosed
    {
        add { menuCounter.LastUnregistered += value; }
        remove { menuCounter.LastUnregistered -= value; }
    }

    public static event RegistrationCounter.RegisteredHandler MenuOpened
    {
        add { menuCounter.FirstRegistered += value; }
        remove { menuCounter.FirstRegistered -= value; }
    }

    public static bool IsMenuOpen => menuCounter.AnyRegistered;

    public static void RegisterClosed()
    {
        menuCounter.Unregister();
    }

    public static void RegisterOpened()
    {
        menuCounter.Register();
    }
}