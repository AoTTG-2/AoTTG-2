using Assets.Scripts.Gamemode.Settings;
using System.Collections.Generic;

public class Level
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string SceneName { get; set; }
    public bool IsCustom { get; set; }
    public string AssetBundle { get; set; }
    public List<GamemodeSettings> Gamemodes { get; set; }
}
