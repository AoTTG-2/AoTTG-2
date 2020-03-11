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
            //HACK, please revert this back to "Test7" when done with #40
            PhotonNetwork.CreateRoom("#40-female-titan"); //, true, true, 2);
            gameObject.SetActive(false);
        }

        public void JoinLobby()
        {
            //HACK, please revert this back to "Test7" when done with #40
            PhotonNetwork.JoinRoom("#40-female-titan");
        }
    }
}
