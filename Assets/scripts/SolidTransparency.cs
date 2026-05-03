using UnityEngine;
using UnityEngine.Rendering;

public class SolidTransparency : MonoBehaviour
{
    [Range(0f, 1f)] public float transparentAlpha = 0.5f;

    private Renderer[] faceRenderers;
    private int activeCount = 0;

    private void Awake()
    {
        faceRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void Register()
    {
        activeCount++;
        if (activeCount == 1)
            SetTransparency(true);
    }

    public void Unregister()
    {
        activeCount--;
        if (activeCount <= 0)
        {
            activeCount = 0;
            SetTransparency(false);
        }
    }

    private void SetTransparency(bool transparent)
    {
        foreach (Renderer r in faceRenderers)
        {
            Material[] mats = r.materials;
            foreach (Material mat in mats)
            {
                if (transparent)
                {
                    mat.SetFloat("_Surface", 1f);
                    mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    mat.renderQueue = (int)RenderQueue.Transparent;
                    Color c = mat.color;
                    c.a = transparentAlpha;
                    mat.color = c;
                }
                else
                {
                    mat.SetFloat("_Surface", 0f);
                    mat.SetInt("_SrcBlend", (int)BlendMode.One);
                    mat.SetInt("_DstBlend", (int)BlendMode.Zero);
                    mat.SetInt("_ZWrite", 1);
                    mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    mat.renderQueue = (int)RenderQueue.Geometry;
                    Color c = mat.color;
                    c.a = 1f;
                    mat.color = c;
                }
            }
            r.materials = mats;
        }
    }
}