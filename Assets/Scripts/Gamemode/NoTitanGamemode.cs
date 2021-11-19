using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Extensions;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Gamemode
{
    public class NoTitanGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType => GamemodeType.NoTitan;
    }


}


