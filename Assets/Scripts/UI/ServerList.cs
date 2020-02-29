using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI
{
    public class ServerList : UiElement    
    {
        public void CreateLobby()
        {
            PhotonNetwork.CreateRoom("Test7"); //, true, true, 2);
        }

        public void JoinLobby()
        {
            PhotonNetwork.JoinRoom("Test7");
        }
    }
}
