using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private NetworkManagerFrostFire networkManager = null;

    [Header("UI")]
    [SerializeField]
    private GameObject landingPagePanel = null;

    public void HostLobby() 
    {
        networkManager.StartHost();

        landingPagePanel.SetActive(false);
    }
}
