using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _username;

    private int _startPlayersCount;
    private PlayerInput _input;

    private Player[] _players = PhotonNetwork.PlayerList;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();

        _startPlayersCount = _players.Length;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _startPlayersCount--;
        RemainingPlayersCheck();
    }

    private void RemainingPlayersCheck()
    {
        if (_startPlayersCount == 1)
        {
            _username.text = _players[0].NickName;
            GameEnd();
        }
    }

    private void GameEnd()
    {
        Destroy(_input);
        _winPanel.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
    }
}
