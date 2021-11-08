using UnityEngine;

public class SpectatorMode
{

    private static SpectatorMovement specMovement;

    public static void Initialize()
    {
        specMovement = GameObject.Find("MainCamera").GetComponent<SpectatorMovement>();
    }

    public static SpectatorMovement Get()
    {
        return specMovement;
    }

    public static bool IsDisable()
    {
        return specMovement.disable;
    }

    public static void Toggle()
    {
        specMovement.disable = !specMovement.disable;
    }

    public static void Disable()
    {
        specMovement.disable = true;
    }

    public static void Enable()
    {
        specMovement.enabled = true;
    }

}
