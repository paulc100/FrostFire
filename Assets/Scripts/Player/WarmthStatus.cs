using UnityEngine;

public class WarmthStatus : MonoBehaviour
{
    /**
     * This code is currently not being used.
     * This used the 'OutlineMask' camera/shader to outline players
     */
    [Header("Player Outline Materials")]
    [SerializeField]
    private Material p1OutlineMaterial = null;
    [SerializeField]
    private Material p2OutlineMaterial = null;

    private OutlineMask outlineShaderReference = null;
    private Warmth warmthReference = null;
    private SplitScreenPlayerController characterControllerReference = null;
    private Material designatedOutlineMaterial = null;

    private void Awake() 
    { 
        warmthReference = GetComponent<Warmth>();

        outlineShaderReference = GameObject.Find("Main Camera").GetComponent<OutlineMask>();

        characterControllerReference = GetComponent<SplitScreenPlayerController>();

        DetermineOutlineMaterial();
    }

    private void Update() 
    {
        if (warmthReference.nearPlayer) {
            Debug.Log("Near Player");
            designatedOutlineMaterial.SetColor("_OutlineColor", new Color(0f, 1f, 0.2f, 1f));
        } else {
            if (warmthReference.nearCampfire) {
                designatedOutlineMaterial.SetColor("_OutlineColor", new Color(0.8f, 1f, 0, 1f));
            } else {
                designatedOutlineMaterial.SetColor("_OutlineColor", new Color(0f, 0.8f, 1f, 1f));
            }
        }
    }

    private void DetermineOutlineMaterial()
    {
        switch(characterControllerReference.pid) 
        {
            case 0:
                designatedOutlineMaterial = p1OutlineMaterial;
                break;
            case 1:
                designatedOutlineMaterial = p2OutlineMaterial;
                break;
        }
    }
}
