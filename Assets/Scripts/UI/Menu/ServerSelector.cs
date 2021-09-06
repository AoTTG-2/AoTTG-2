using Assets.Scripts.Room;
using Assets.Scripts.Services;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI class for the "Lobby" which contains a list of all available rooms.
    /// </summary>
    public class ServerSelector : UiNavigationElement
    {
        public override void Back()
        {
            base.Back();
            PhotonNetwork.Disconnect();
        }
    }
}