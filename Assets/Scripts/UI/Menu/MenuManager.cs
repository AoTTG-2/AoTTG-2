using Assets.Scripts.UI.Menu;
using System;
using System.Collections.Generic;

/// <summary>
/// A static MenuManager which keeps track over how many menus are open. This is used to determine cursor behavior
/// </summary>
public static class MenuManager
{
    private static readonly RegistrationCounter menuCounter = new RegistrationCounter();
    private static readonly List<IUiElement> menus = new List<IUiElement>();

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

    public static bool IsAnyMenuOpen => menuCounter.AnyRegistered;

    public static bool IsMenuOpen(Type targetMenuType)
    {
        foreach (var menu in menus)
        {
            if (menu.GetType().Equals(targetMenuType))
            {
                return true;
            }
        }
        return false;
    }

    public static void RegisterClosed(IUiElement menu)
    {
        menuCounter.Unregister();
        menus.Remove(menu);
    }

    public static void RegisterOpened(IUiElement menu)
    {
        menuCounter.Register();
        menus.Add(menu);
    }
}