namespace Assets.Scripts.CustomLogic
{
    public class PlayerLogicManager
    {
        public void Kill(int playerId, string reason = "")
        {
            if (FengGameManagerMKII.heroHash.ContainsKey(playerId))
            {
                Hero hero = (Hero)FengGameManagerMKII.heroHash[playerId];
                hero.markDie();
                hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, reason});
            }
        }
    }
}
