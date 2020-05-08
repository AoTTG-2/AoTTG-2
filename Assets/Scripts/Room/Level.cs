using Assets.Scripts.Gamemode;
using System.Collections.Generic;
using Assets.Scripts.Gamemode.Settings;

public class Level
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string SceneName { get; set; }
    public List<GamemodeSettings> Gamemodes { get; set; }
}
