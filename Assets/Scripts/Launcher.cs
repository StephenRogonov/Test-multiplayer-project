using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _roomsList;
    [SerializeField] private GameObject _roomButtonPrefab;
    [SerializeField] private Transform _playersList;
    [SerializeField] private GameObject _playerNamePrefab;
    [SerializeField] private GameObject _startGameButton;

    private void Start()
    {
        instance = this;
        Debug.Log("Connecting to Master Server");
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby");
        MenuManager.instance.OpenMenu("Main");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(_roomNameInputField.text);
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("Room");

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < _playersList.childCount; i++)
        {
            Destroy(_playersList.GetChild(i).gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(_playerNamePrefab, _playersList).GetComponent<PlayersListItem>().SetUp(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorText.text = "Error: " + message;
        MenuManager.instance.OpenMenu("Error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("Main");
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < _roomsList.childCount; i++)
        {
            Destroy(_roomsList.GetChild(i).gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(_roomButtonPrefab, _roomsList).GetComponent<RoomsListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_playerNamePrefab, _playersList).GetComponent<PlayersListItem>().SetUp(newPlayer);
    }
}
