using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomsListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomName;

    public RoomInfo roomInfo;

    public void SetUp(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
        _roomName.text = this.roomInfo.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(roomInfo);
    }
}
