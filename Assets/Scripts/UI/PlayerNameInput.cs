using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] 
    private InputField nameInput = null;
    [SerializeField]
    private Button continueButton = null;

    public static string PlayerName { get; private set; }
    private const string playerPrefsNameKey = "playerName";

    private void Start() => SetupInputField();

    private void SetupInputField() 
    {
        if (!PlayerPrefs.HasKey(playerPrefsNameKey))
            return;

        string defaultName = PlayerPrefs.GetString(playerPrefsNameKey);

        nameInput.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        PlayerName = nameInput.text;

        PlayerPrefs.SetString(playerPrefsNameKey, PlayerName);
    }
}
