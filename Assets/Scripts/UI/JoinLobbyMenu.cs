using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField]
    private NetworkManagerFrostFire networkManager = null;

    [Header("UI")]
    [SerializeField]
    private GameObject landingPagePanel = null;
    [SerializeField]
    private InputField ipAddressInputField = null;
    [SerializeField]
    private Button joinButton = null;

    private void OnEnable()
    {
        NetworkManagerFrostFire.OnClientConnected += HandleClientConnected;
        NetworkManagerFrostFire.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerFrostFire.OnClientConnected -= HandleClientConnected;
        NetworkManagerFrostFire.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
