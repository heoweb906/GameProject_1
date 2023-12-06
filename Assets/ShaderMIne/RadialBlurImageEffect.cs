using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBlurImageEffect : MonoBehaviour
{
    public float blurSize = 0.1f;
    public Vector4 blurCenterPos = new Vector4(0.5f, 0.5f, 0f, 0f);

    [Range(1, 48)]
    public int samples;

    public Material radialBlurMaterial = null;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (blurSize > 0.0f)
        {
            radialBlurMaterial.SetFloat("_BlurSize", blurSize);
            radialBlurMaterial.SetVector("_BlurCenterPos", blurCenterPos);
            radialBlurMaterial.SetInt("_Samples", samples);
            Graphics.Blit(source, destination, radialBlurMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}