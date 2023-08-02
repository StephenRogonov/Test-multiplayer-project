using Photon.Pun;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
