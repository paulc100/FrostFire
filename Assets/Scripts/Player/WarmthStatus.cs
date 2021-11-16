using UnityEditor;
using UnityEngine;

public class WarmthStatus : MonoBehaviour
{
    [SerializeField]
    private OutlineMask outlineShaderReference = null;
    private Warmth warmthReference = null;

    private void Awake() => warmthReference = GetComponent<Warmth>();

    private void Update() 
    {
        // TODO: Modify logic for changing glow color
        if (warmthReference.nearCampfire) {
                outlineShaderReference.outlineMaterial.SetColor("_OutlineColor", new Color(0.8f, 1f, 0, 1f));
        } else {
            outlineShaderReference.outlineMaterial.SetColor("_OutlineColor", new Color(0f, 0.8f, 1f, 1f));
        }
    }
}
