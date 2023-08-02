using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] private TMP_InputField _createRoomNameInputField;
    [SerializeField] private TMP_InputField _joinRoomNameInputField;
    [SerializeField] private TMP_Text _createRoomErrorText;
    [SerializeField] private TMP_Text _joinRoomErrorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _playersList;
    [SerializeField] private GameObject _playerNamePrefab;
    [SerializeField] private GameObject _startGameButton;

    private Player[] _players;

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.ConnectUsingSettings();
    }

    //private void Start()
    //{
    //    Debug.Log("Connecting to Master");
    //    //PhotonNetwork.ConnectUsingSettings();
    //    //if (!PhotonNetwork.IsConnected)
    //    //{

    //    //    Debug.Log("Connecting to Master");
    //    //    PhotonNetwork.ConnectUsingSettings();
    //    //}
    //}

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.Instance.OpenMenu("Main");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_createRoomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(_createRoomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        _players = PhotonNetwork.PlayerList;

        foreach (Transform child in _playersList)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _players.Length; i++)
        {
            Instantiate(_playerNamePrefab, _playersList).GetComponent<PlayersListItem>().SetUp(_players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        _startGameButton.GetComponent<Button>().interactable = false;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _createRoomErrorText.text = message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _joinRoomErrorText.text = message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Main");
    }
    
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_joinRoomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.JoinRoom(_joinRoomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_playerNamePrefab, _playersList).GetComponent<PlayersListItem>().SetUp(newPlayer);

        _players = PhotonNetwork.PlayerList;

        if (_players.Length > 1) 
        {
            _startGameButton.GetComponent<Button>().interactable = true;
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}
