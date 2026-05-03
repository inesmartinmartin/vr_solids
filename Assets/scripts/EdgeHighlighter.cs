using System.Collections.Generic;
using UnityEngine;

public class EdgeHighlighter : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform[] vertexLocators;
    public int[,] edgePairs;
    public GameObject edgeCylinderPrefab;
    public float edgeRadius = 0.005f;

    private List<GameObject> spawnedEdges = new List<GameObject>();
    private bool isActive = false;
    private float lastToggleTime = 0f;
    private float toggleCooldown = 0.5f;

    public void Toggle()
    {
        if (Time.time - lastToggleTime < toggleCooldown) return;
        lastToggleTime = Time.time;

        Debug.Log("EdgeHighlighter.Toggle() called, isActive = " + isActive);
        if (isActive) HideEdges();
        else ShowEdges();
    }

    private void ShowEdges()
    {
        isActive = true;

        if (vertexLocators == null || vertexLocators.Length == 0)
        {
            Debug.LogError("EdgeHighlighter: vertexLocators is empty on " + gameObject.name);
            return;
        }

        if (edgeCylinderPrefab == null)
        {
            Debug.LogError("EdgeHighlighter: edgeCylinderPrefab not assigned on " + gameObject.name);
            return;
        }

        if (edgePairs == null || edgePairs.GetLength(0) == 0)
        {
            Debug.LogError("EdgeHighlighter: edgePairs not defined on " + gameObject.name);
            return;
        }

        GetComponent<SolidTransparency>().Register();

        SolidIdentity identity = GetComponent<SolidIdentity>();
        if (identity == null) { Debug.LogError("EdgeHighlighter: no SolidIdentity on " + gameObject.name); return; }
        Color accent = identity.data.solidColor;

        for (int i = 0; i < edgePairs.GetLength(0); i++)
        {
            int indexA = edgePairs[i, 0];
            int indexB = edgePairs[i, 1];

            if (indexA >= vertexLocators.Length || indexB >= vertexLocators.Length)
            {
                Debug.LogWarning("EdgeHighlighter: edge pair [" + indexA + "," + indexB + "] out of range");
                continue;
            }

            Vector3 posA = vertexLocators[indexA].position;
            Vector3 posB = vertexLocators[indexB].position;

            Vector3 midpoint = (posA + posB) / 2f;
            float length = Vector3.Distance(posA, posB);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, posB - posA);

            GameObject cylinder = Instantiate(
                edgeCylinderPrefab,
                midpoint,
                rotation,
                this.transform
            );

            float worldRadius = edgeRadius / this.transform.lossyScale.x;
            float worldLength = length / this.transform.lossyScale.x;
            cylinder.transform.localScale = new Vector3(worldRadius, worldLength / 2f, worldRadius);

            Renderer r = cylinder.GetComponent<Renderer>();
            if (r != null)
            {
                Material mat = r.material;
                mat.color = accent;
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", accent * 1.5f);
            }

            spawnedEdges.Add(cylinder);
        }

        Debug.Log("EdgeHighlighter: spawned " + spawnedEdges.Count + " edges");
    }

    private void HideEdges()
    {
        isActive = false;
        GetComponent<SolidTransparency>().Unregister();

        foreach (GameObject e in spawnedEdges)
            Destroy(e);

        spawnedEdges.Clear();
        Debug.Log("EdgeHighlighter: edges hidden");
    }
}