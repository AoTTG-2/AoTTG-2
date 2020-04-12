using Assets.Scripts.Gamemode;
using System.Collections.Generic;

public class Level
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string SceneName { get; set; }
    public List<GamemodeBase> Gamemodes { get; set; }
}
