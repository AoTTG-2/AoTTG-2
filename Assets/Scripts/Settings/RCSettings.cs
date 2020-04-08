using System;

[Obsolete("Please is Gamemode instead")]
public static class RCSettings
{
    public static int ahssReload;
    public static float aRate;
    public static int banEren;
    public static int bombMode;
    public static float cRate;
    public static int damageMode;
    public static int deadlyCannons;
    public static int disableRock;
    public static int endlessMode;
    public static int explodeMode;
    public static int friendlyMode;
    public static int gameType;
    public static int globalDisableMinimap; // Enable Minimap
    public static int healthLower; // Min Health
    public static int healthMode; // 1 = random health between healthLower - healthUpper, 2 = health is based on titan size. (size / 4) * random health
    public static int healthUpper; // Max Health
    public static int infectionMode; //Infection mode 1 = enable, 0 = disable
    public static float jRate;
    public static string motd;
    public static float nRate;
    public static int pointMode;
    public static float pRate;
    public static int racingStatic;
    public static float sizeLower; //Minimun titan size
    public static int sizeMode; //Are custom sizes enabled? If true, then sizeLower and sizeUpper are used.
    public static float sizeUpper; //Max titan size
    public static int spawnMode;
    public static int teamMode;
    public static int titanCap;
}