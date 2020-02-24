using System;
using System.Collections;
using UnityEngine;

public class PanelMultiJoin : MonoBehaviour
{
    private int currentPage = 1;
    private float elapsedTime = 10f;
    private string filter = string.Empty;
    private ArrayList filterRoom;
    public GameObject[] items;
    private int totalPage = 1;

    public void connectToIndex(int index, string roomName)
    {
        int num = 0;
        for (num = 0; num < 10; num++)
        {
            this.items[num].SetActive(false);
        }
        num = (10 * (this.currentPage - 1)) + index;
        char[] separator = new char[] { "`"[0] };
        string[] strArray = roomName.Split(separator);
        if (strArray[5] != string.Empty)
        {
            PanelMultiJoinPWD.Password = strArray[5];
            PanelMultiJoinPWD.roomName = roomName;
            NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, true);
            NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, false);
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    private string getServerDataString(RoomInfo room)
    {
        char[] separator = new char[] { "`"[0] };
        string[] strArray = room.name.Split(separator);
        object[] objArray1 = new object[] { !(strArray[5] == string.Empty) ? "[PWD]" : string.Empty, strArray[0], "/", strArray[1], "/", strArray[2], "/", strArray[4], " ", room.playerCount, "/", room.maxPlayers };
        return string.Concat(objArray1);
    }

    private void OnDisable()
    {
    }

    private void OnEnable()
    {
        this.currentPage = 1;
        this.totalPage = 0;
        this.refresh();
    }

    private void OnFilterSubmit(string content)
    {
        this.filter = content;
        this.updateFilterRooms();
        this.showlist();
    }

    public void pageDown()
    {
        this.currentPage++;
        if (this.currentPage > this.totalPage)
        {
            this.currentPage = 1;
        }
        this.showServerList();
    }

    public void pageUp()
    {
        this.currentPage--;
        if (this.currentPage < 1)
        {
            this.currentPage = this.totalPage;
        }
        this.showServerList();
    }

    public void refresh()
    {
        this.showlist();
    }

    private void showlist()
    {
        if (this.filter == string.Empty)
        {
            if (PhotonNetwork.GetRoomList().Length > 0)
            {
                this.totalPage = ((PhotonNetwork.GetRoomList().Length - 1) / 10) + 1;
            }
            else
            {
                this.totalPage = 1;
            }
        }
        else
        {
            this.updateFilterRooms();
            if (this.filterRoom.Count > 0)
            {
                this.totalPage = ((this.filterRoom.Count - 1) / 10) + 1;
            }
            else
            {
                this.totalPage = 1;
            }
        }
        if (this.currentPage < 1)
        {
            this.currentPage = this.totalPage;
        }
        if (this.currentPage > this.totalPage)
        {
            this.currentPage = 1;
        }
        this.showServerList();
    }

    private void showServerList()
    {
        if (PhotonNetwork.GetRoomList().Length != 0)
        {
            int index = 0;
            if (this.filter == string.Empty)
            {
                for (index = 0; index < 10; index++)
                {
                    int num2 = (10 * (this.currentPage - 1)) + index;
                    if (num2 < PhotonNetwork.GetRoomList().Length)
                    {
                        this.items[index].SetActive(true);
                        this.items[index].GetComponentInChildren<UILabel>().text = this.getServerDataString(PhotonNetwork.GetRoomList()[num2]);
                        this.items[index].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = PhotonNetwork.GetRoomList()[num2].name;
                    }
                    else
                    {
                        this.items[index].SetActive(false);
                    }
                }
            }
            else
            {
                for (index = 0; index < 10; index++)
                {
                    int num3 = (10 * (this.currentPage - 1)) + index;
                    if (num3 < this.filterRoom.Count)
                    {
                        RoomInfo room = (RoomInfo) this.filterRoom[num3];
                        this.items[index].SetActive(true);
                        this.items[index].GetComponentInChildren<UILabel>().text = this.getServerDataString(room);
                        this.items[index].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = room.name;
                    }
                    else
                    {
                        this.items[index].SetActive(false);
                    }
                }
            }
            GameObject.Find("LabelServerListPage").GetComponent<UILabel>().text = this.currentPage + "/" + this.totalPage;
        }
        else
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                this.items[i].SetActive(false);
            }
            GameObject.Find("LabelServerListPage").GetComponent<UILabel>().text = this.currentPage + "/" + this.totalPage;
        }
    }

    private void Start()
    {
        int index = 0;
        for (index = 0; index < 10; index++)
        {
            this.items[index].SetActive(true);
            this.items[index].GetComponentInChildren<UILabel>().text = string.Empty;
            this.items[index].SetActive(false);
        }
    }

    private void Update()
    {
        this.elapsedTime += Time.deltaTime;
        if (this.elapsedTime > 1f)
        {
            this.elapsedTime = 0f;
            this.showlist();
        }
    }

    private void updateFilterRooms()
    {
        this.filterRoom = new ArrayList();
        if (this.filter != string.Empty)
        {
            foreach (RoomInfo info in PhotonNetwork.GetRoomList())
            {
                if (info.name.ToUpper().Contains(this.filter.ToUpper()))
                {
                    this.filterRoom.Add(info);
                }
            }
        }
    }
}

