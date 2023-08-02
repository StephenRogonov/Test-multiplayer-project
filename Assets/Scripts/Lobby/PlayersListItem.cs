using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayersListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _playerName;
    private Player _player;

    public void SetUp(Player player)
    {
        _player = player;
        _playerName.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
