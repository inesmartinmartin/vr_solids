using System.Collections.Generic;
using UnityEngine;

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

       GetComponent<SolidTransparency>().Register();

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

        GetComponent<SolidTransparency>().Unregister();
        foreach (GameObject s in spawnedSpheres)
            Destroy(s);

        spawnedSpheres.Clear();
        Debug.Log("VertexHighlighter: vertices hidden");
    }
}