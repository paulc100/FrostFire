using UnityEngine;

public class OutlineMask : MonoBehaviour
{
    [Header("Shaders")]
    [SerializeField]
    private Shader DrawAsSolidColor;
    [SerializeField]
    private Shader Outline;

    public Material outlineMaterial;
    private Camera tempCamera;
    private float[] kernel;

    private void Start()
    {
        // Setup second camera to render outlined objects
        tempCamera = new GameObject().AddComponent<Camera>();

        kernel = GaussianKernel.Calculate(5, 21);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) 
    {
        tempCamera.CopyFrom(Camera.current);
        tempCamera.backgroundColor = Color.black;
        tempCamera.clearFlags = CameraClearFlags.Color;

        // Cull anything that isn't in the outline layer
        tempCamera.cullingMask = 1 << LayerMask.NameToLayer("Outline");

        // Allocate video memory for texture
        var renderTexture = RenderTexture.GetTemporary(src.width, src.height, 0, RenderTextureFormat.R8);
        tempCamera.targetTexture = renderTexture;

        // Use a simple shader to redraw objects
        tempCamera.RenderWithShader(DrawAsSolidColor, "");

        outlineMaterial.SetFloatArray("_Kernel", kernel);
        outlineMaterial.SetInt("_KernelWidth", kernel.Length);
        outlineMaterial.SetTexture("_SceneTex", src);

        renderTexture.filterMode = FilterMode.Point;

        Graphics.Blit(renderTexture, dest, outlineMaterial);
        tempCamera.targetTexture = src;
        RenderTexture.ReleaseTemporary(renderTexture);
    }
}
