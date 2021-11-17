using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField]
    private List<GameObject> availableCharacters = new List<GameObject>();

    private PlayerInputManager playerInputManager;
    private int characterIndex = 0;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.playerPrefab = availableCharacters[characterIndex];
        characterIndex++;
    }

    public void SwitchNextSpawnCharacter()
    {
        playerInputManager.playerPrefab = availableCharacters[characterIndex];
        characterIndex++;
    }
}
