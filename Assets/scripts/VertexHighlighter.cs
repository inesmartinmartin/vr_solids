using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VertexHighlighter : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform[] vertexLocators;
    public GameObject vertexSpherePrefab;
    public float sphereRadius = 0.04f;

    private List<GameObject> spawnedSpheres = new List<GameObject>();
    private bool isActive = false;

    private float lastToggleTime = 0f;
    private float toggleCooldown = 0.5f;

    [Header("Transparency")]
    [Range(0f, 1f)] public float transparentAlpha = 0.6f;

    private Renderer[] faceRenderers;

    private void Awake()
    {
        faceRenderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Log("VertexHighlighter: found " + faceRenderers.Length + " renderers:");
        foreach (Renderer r in faceRenderers)
            Debug.Log("  → " + r.gameObject.name + " | material: " + r.material.name);
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
                    // Switch to transparent mode
                    mat.SetFloat("_Surface", 1f);                           // 0 = Opaque, 1 = Transparent
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    Color c = mat.color;
                    c.a = transparentAlpha;
                    mat.color = c;
                }
                else
                {
                    // Switch back to opaque mode
                    mat.SetFloat("_Surface", 0f);
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    mat.SetInt("_ZWrite", 1);
                    mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

                    Color c = mat.color;
                    c.a = 1f;
                    mat.color = c;
                }
            }
            r.materials = mats;
        }
    }

    public void Toggle()
    {
        if (Time.time - lastToggleTime < toggleCooldown) return;
        lastToggleTime = Time.time;
        
        Debug.Log("VertexHighlighter.Toggle() called, isActive = " + isActive);
        if (isActive) HideVertices();
        else ShowVertices();
    }

    private void ShowVertices()
    {
        isActive = true;

        if (vertexLocators == null || vertexLocators.Length == 0)
        {
            Debug.LogError("VertexHighlighter: vertexLocators array is empty or null on " + gameObject.name);
            return;
        }

        if (vertexSpherePrefab == null)
        {
            Debug.LogError("VertexHighlighter: vertexSpherePrefab is not assigned on " + gameObject.name);
            return;
        }

        SetTransparency(true); // <- add this

        SolidIdentity identity = GetComponent<SolidIdentity>();
        if (identity == null) { Debug.LogError("VertexHighlighter: no SolidIdentity on " + gameObject.name); return; }

        Color accent = identity.data.solidColor;

        foreach (Transform locator in vertexLocators)
        {
            if (locator == null) { Debug.LogWarning("VertexHighlighter: a vertexLocator entry is null, skipping"); continue; }

            GameObject sphere = Instantiate(
                vertexSpherePrefab,
                locator.position,
                Quaternion.identity,
                this.transform
            );

            float worldRadius = sphereRadius / this.transform.lossyScale.x;
            sphere.transform.localScale = Vector3.one * worldRadius;

            Renderer r = sphere.GetComponent<Renderer>();
            if (r == null) { Debug.LogWarning("VertexHighlighter: sphere prefab has no Renderer"); continue; }

            Material mat = r.material;
            mat.color = accent;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", accent * 1.5f);

            spawnedSpheres.Add(sphere);
        }

        Debug.Log("VertexHighlighter: done, spawned " + spawnedSpheres.Count + " spheres");
    }

    private void HideVertices()
    {
        isActive = false;

        SetTransparency(false); // <- add this

        foreach (GameObject s in spawnedSpheres)
            Destroy(s);

        spawnedSpheres.Clear();
        Debug.Log("VertexHighlighter: vertices hidden");
    }
}