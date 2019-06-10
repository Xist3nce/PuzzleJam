using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CamLightEffect : MonoBehaviour
{
    Camera mainCamera;
    public Camera lightsCamera;
    public RenderTexture multiplyTexture;
    Material material;
    int resolutionHash = 0;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        material = new Material(Shader.Find("Hidden/MultiplyShader"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }

    void OnPreCull()
    {
        int newResolutionHash = Screen.width * Screen.height;
        if (resolutionHash != newResolutionHash)
        {
            multiplyTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBHalf);
            lightsCamera.targetTexture = multiplyTexture;
            material.SetTexture("_MultTex", multiplyTexture);
            resolutionHash = newResolutionHash;
        }
        lightsCamera.orthographicSize = mainCamera.orthographicSize;
    }
}