using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        public EndlessGamemode()
        {
            GamemodeType = GamemodeType.Endless;
            RespawnMode = RespawnMode.NEVER;
            Pvp = PvpMode.Disabled;
            Titans = 10;
        }

        public override void OnTitanKilled(string titanName)
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
