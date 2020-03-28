using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomRow : MonoBehaviour
{
    public string Room;
    public string DisplayName;
    public bool IsJoinable = true;
    // Use this for initialization
	void Start () 
    {
        GetComponentInChildren<Text>().text = DisplayName;
    }

    public void JoinLobby()
    {
        if (!IsJoinable) return;
        PhotonNetwork.JoinRoom(Room);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        gameObject.gameObject.gameObject.gameObject.gameObject.SetActive(false);
        AottgUi.TestSpawn();
    }
}
