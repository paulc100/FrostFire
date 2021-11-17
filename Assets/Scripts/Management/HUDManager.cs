using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerHUD> playerHUDS = new List<PlayerHUD>();
    private int playerNumberIndex = 0;

    public void AddPlayerReferenceToHUD(GameObject playerReference)
    {
        PlayerHUD newPlayerHUD = playerHUDS[playerNumberIndex];

        // Swap inactive HUD with active HUD
        newPlayerHUD.inactiveHUD.SetActive(false);
        newPlayerHUD.activeHUD.SetActive(true);

        // Pair player warmth to HUD
        newPlayerHUD.playerWarmthReference = playerReference.GetComponent<Warmth>();

        playerNumberIndex++;
    }
}
