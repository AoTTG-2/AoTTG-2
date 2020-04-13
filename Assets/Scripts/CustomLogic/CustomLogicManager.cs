using System.Collections.Generic;
using System.Linq;
using CustomLogic;
using UnityEngine;

namespace Assets.Scripts.CustomLogic
{
    public class CustomLogicManager : MonoBehaviour
    {
        private FengGameManagerMKII gameManager;
        private InRoomChat inRoomChat;

        public PlayerLogicManager PlayerLogicManager { get; set; } = new PlayerLogicManager();

        private void Start()
        {
            gameManager = FengGameManagerMKII.instance;
            inRoomChat = gameManager.chatRoom;
            Events.Setup(this);
        }

        public bool IsMasterClient() 
        {
            return PhotonNetwork.isMasterClient;
        }

        public List<Player> GetAllPlayers()
        {
            var photonPlayers = PhotonNetwork.playerList.ToList();
            var players = new List<Player>();
            foreach (var player in photonPlayers)
            {
                players.Add(new Player(player));
            }
            return players;
        }

        //public void Kill(int playerId, string reason = "")
        //{
        //    Debug.LogError($"Player ID: {playerId}, with reasion {reason}");
        //    if (FengGameManagerMKII.heroHash.ContainsKey(playerId))
        //    {
        //        Hero hero = (Hero)FengGameManagerMKII.heroHash[playerId];
        //        hero.markDie();
        //        hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, reason });
        //    }
        //}

        public void SendChatMessage(string message, bool everyone = true)
        {
            if (!everyone)
            {
                inRoomChat.addLINE(message);
            }
            else
            {
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, message, "");
            }
        }
    }
}
