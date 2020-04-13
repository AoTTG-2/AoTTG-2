namespace Assets.Scripts.CustomLogic
{
    public class Player
    {

        public Player(PhotonPlayer photonPlayer)
        {
            PlayerId = photonPlayer.ID;
        }

        public int PlayerId;
        public void Spawn()
        {

        }

        public void Spawn(int x, int y, int z)
        {

        }
    }
}
