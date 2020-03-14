using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        public EndlessGamemode()
        {
            GamemodeType = GamemodeType.Endless;
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
