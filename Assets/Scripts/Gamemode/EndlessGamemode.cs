﻿namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        public EndlessGamemode()
        {
            GamemodeType = GamemodeType.Endless;
            RespawnMode = RespawnMode.NEVER;
            AllowPlayerTitans = true;
            Pvp = false;
        }

        public override void OnTitanKilled(string titanName, bool onPlayerLeave)
        {
            if (!onPlayerLeave)
            {
                HumanScore++;
                int num2 = 90;
                if (FengGameManagerMKII.instance.difficulty == 1)
                {
                    num2 = 70;
                }
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", num2, 1, false);
            }
        }
    }
}
