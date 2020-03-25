using UnityEngine;
using UnityEngine.UI;

public class RoomRow : MonoBehaviour
{
    public string Room;
    public bool IsJoinable = true;
    // Use this for initialization
	void Start () 
    {
        GetComponentInChildren<Text>().text = Room;
    }

    public void JoinLobby()
    {
        if (!IsJoinable) return;
        PhotonNetwork.JoinRoom(Room);
    }
}
